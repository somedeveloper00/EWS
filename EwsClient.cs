using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using EWS.Interfaces;

namespace EWS
{
    /// <summary>
    /// Represents a client for the EWS protocol.
    /// </summary>
    public class EwsClient
    {
        /// <summary>
        /// The interval between reconnect attempts.
        /// </summary>
        public TimeSpan reconnectInterval = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The timeout for connecting to the server.
        /// </summary>
        public TimeSpan connectTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// The byte that marks the end of a message.
        /// </summary>
        private const byte EndOfMessageByte = 0x00;

        /// <summary>
        /// The underlying Socke<see cref="Socket"/>.
        /// </summary>
        private Socket _socket;

        /// <summary>
        /// The size of the buffer used to read data from the stream.
        /// </summary>
        private readonly int _bufferSize;

        /// <summary>
        /// The listeners for each event id.
        /// </summary>
        private readonly List<IEwsEventListener>[] _listeners = new List<IEwsEventListener>[byte.MaxValue];

        /// <summary>
        /// The endpoint that the client connects to.
        /// </summary>
        private readonly EndPoint _endpoint;

        /// <summary>
        /// The protocol that the client uses.
        /// </summary>
        private readonly ProtocolType _protocol;

        /// <summary>
        /// Raised when an error occurs.
        /// </summary>
        public event LogErrorDelegate LogError;

        /// <summary>
        /// Raised when the client is connected.
        /// </summary>
        public event EmptyDelegate Connected;

        /// <summary>
        /// Raised when the client is disconnected.
        /// </summary>
        public event EmptyDelegate Disconnected;

        /// <summary>
        /// The encryption algorithm used for sending and receiving data. (null for no encryptionإ)
        /// </summary>
        public IEncryption encryption;

        /// <summary>
        /// The stream hook used to hook into the client stream. (for sending data, the hook is called 
        /// after the encryption algorithm is applied, but for receiving data, the hook is called before the 
        /// decryption algorithm is applied). (null for no hook)
        /// </summary>
        public IEwsStreamManip streamManip;

        /// <summary>
        /// preprocessor for when receiving new events. if its null, the listener will be executed right away, otherwise, the 
        /// execution of listener is at this class's responsibility.
        /// </summary>
        public IListenerPreprocess listenerPreprocess;

        private CancellationTokenSource _cts;

        private readonly bool _keepAlive;

        /// <summary>
        /// uses the given client to create a new EwsClient.
        /// </summary>
        internal EwsClient(Socket socket, int bufferSize = 1024)
        {
            _socket = socket;
            _bufferSize = bufferSize;
            _endpoint = socket.RemoteEndPoint;
            _protocol = socket.ProtocolType;
            _keepAlive = (bool)socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive);
        }

        /// <summary>
        /// Creates a new EwsClient.
        /// </summary>
        public EwsClient(string host, int port, ProtocolType protocol = ProtocolType.Tcp,
            int bufferSize = 1024,
            bool keepAlive = true)
        {
            _bufferSize = bufferSize;
            _protocol = protocol;
            _keepAlive = keepAlive;
            _endpoint = new IPEndPoint(IPAddress.Parse(host), port);
        }

        /// <summary>
        /// Connects to the given host and port. Use events <see cref="Connected"/> and <see cref="Disconnected"/> 
        /// to listen for the result. When <see cref="IsConnected"/> is true, you don't need to use this.
        /// </summary>
        public async Task ConnectAsync()
        {
            if (IsConnected())
            {
                LogError?.Invoke("client already connected");
                return;
            }

            try
            {
                // make cancellation token temporarily serve for connection
                _cts?.Cancel();
                _cts = new();
                _cts.CancelAfter(connectTimeout);

                // connect
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, _protocol);
                if (_keepAlive)
                {
                    _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                }
                await _socket.ConnectAsync(_endpoint);


                if (_cts.IsCancellationRequested)
                {
                    Close();
                    return;
                }

                // send EWS secret
                await _socket.SendAsync(EwsUtilities.EwsSecret, SocketFlags.None, _cts.Token);
                if (_cts.IsCancellationRequested)
                {
                    Close();
                    return;
                }

                // listen for server's acceptance
                var buffers = new byte[EwsUtilities.SecretAccepted.Length];
                var c = await _socket.ReceiveAsync(buffers, SocketFlags.None);

                if (_cts.IsCancellationRequested)
                {
                    Close();
                    return;
                }

                if (c == EwsUtilities.SecretAccepted.Length &&
                    buffers.SequenceEqual(EwsUtilities.SecretAccepted, 0, c - 1))
                {
                    // server has accepted connection
                    Connected?.Invoke();
                    StartCommunicationLoop();
                }
                else
                {
                    // server has refused connection
                    Close();
                    return;
                }

            }
            catch (Exception ex)
            {
                LogError?.Exception(ex);

                // retry
                if (reconnectInterval.Ticks > 0)
                {
                    await ReconnectAsync();
                }
            }

            void Close()
            {
                _socket.Close();
                _socket.Dispose();
            }
        }

        /// <summary>
        /// Closes the connection. Use event <see cref="Disconnected"/> to listen for the result. When 
        /// <see cref="IsConnected"/> is false, you don't need to use this.
        /// </summary>
        public void Close()
        {
            if (IsConnected())
            {
                _cts.Cancel();
                _socket.Dispose();
            }
        }

        /// <summary>
        /// Returns true if the client is connected.
        /// </summary>
        public bool IsConnected()
        {
            return _cts?.IsCancellationRequested == false && _socket?.Connected == true;
        }

        /// <summary>
        /// Adds a listener for the given event id. Use <see cref="RemoveListener"/> to remove the listener.
        /// </summary>
        public void AddListener(byte eventId, IEwsEventListener listener)
        {
            EnsureListenerForEventIdIsNotNull(eventId);
            _listeners[eventId].Add(listener);
        }

        /// <summary>
        /// Removes the given listener for the given event id.
        /// </summary>
        public void RemoveListener(byte eventId, IEwsEventListener listener)
        {
            EnsureListenerForEventIdIsNotNull(eventId);
            _listeners[eventId].Remove(listener);
        }

        /// <summary>
        /// Removes all listeners for the given event id.
        /// </summary>
        public void ClearEventListeners(byte eventId)
        {
            EnsureListenerForEventIdIsNotNull(eventId);
            _listeners[eventId].Clear();
        }

        /// <summary>
        /// Sends the given message to the server. Use <see cref="IsConnected"/> to check if the 
        /// client is connected. (The message doesn't need to end with <see cref="EndOfMessageByte"/>!)
        /// </summary>
        public void Send(byte eventId, byte[] message)
        {
            if (!IsConnected())
            {
                LogError?.Invoke("client is not connected");
                return;
            }
            if (eventId == 0x00)
            {
                LogError?.Invoke("cannot send message with event id 0x00");
                return;
            }

            // hook
            if (encryption is not null)
            {
                encryption.Encrypt(this, ref message, out var errorMessage);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    LogError?.Invoke(errorMessage);
                    return;
                }
            }

            var data = new List<byte>(message.Length + 2)
            {
                eventId
            };
            data.AddRange(message);
            data.Add(EndOfMessageByte);

            // hook
            streamManip?.OnSend(data.ToArray());

            _socket.SendAsync(data.ToArray(), SocketFlags.None, _cts.Token);
        }

        /// <summary>
        /// Sends the given message to the server. Use <see cref="IsConnected"/> to check if the
        /// </summary>
        private async Task ReconnectAsync()
        {
            _cts?.Cancel();
            _socket.Dispose();
            await Task.Delay(reconnectInterval);
            await ConnectAsync();
        }

        /// <summary>
        /// Stops the socket communication loop.
        /// </summary>
        private void StartCommunicationLoop()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var buffer = new byte[_bufferSize];
                var data = new List<byte>(_bufferSize);

                // listen for messages
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        var c = await _socket.ReceiveAsync(buffer, SocketFlags.None, _cts.Token);

                        // if server has disconnected, c is 0
                        if (c == 0)
                        {
                            await ReconnectAsync();
                            return;
                        }

                        if (_cts.IsCancellationRequested)
                        {
                            _socket.Close();
                            _socket.Dispose();
                            Disconnected?.Invoke();
                            return;
                        }

                        // hook
                        streamManip?.OnRead(buffer, c);

                        // handle message
                        for (int i = 0; i < c; i++)
                        {
                            data.Add(buffer[i]);
                        }

                        if (data.Count > 0 && data[^1] == EndOfMessageByte)
                        {
                            HandleMessage(data);
                            data.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError?.Invoke("{0}\n{1}", ex.Message, ex.StackTrace);
                    }
                }
            });
        }

        /// <summary>
        /// Handles the given message. Calls appropriate <see cref="_listeners"/>
        /// </summary>
        private void HandleMessage(List<byte> data)
        {
            byte eventId = data[0];
            var message = data.Count <= 2 ? Array.Empty<byte>() : data.Skip(1).Take(data.Count - 1).ToArray();
            if (encryption is not null)
            {
                encryption.Decrypt(this, ref message, out var errorMessage);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    LogError?.Invoke(errorMessage);
                    return;
                }
            }
            EnsureListenerForEventIdIsNotNull(0);
            foreach (var listener in _listeners[0])
            {
                ExecuteListener(message, listener);
            }

            if (eventId != 0)
            {
                EnsureListenerForEventIdIsNotNull(eventId);
                foreach (var listener in _listeners[eventId])
                {
                    ExecuteListener(message, listener);
                }
            }
        }

        /// <summary>
        /// Executes the given listener. (Catches exceptions)
        /// </summary>
        private void ExecuteListener(byte[] message, IEwsEventListener listener)
        {
            try
            {
                if (listenerPreprocess is not null)
                {
                    listenerPreprocess.ExecuteNewEvent(this, message, listener);
                }
                else
                {
                    listener.Process(this, message);
                }
            }
            catch (Exception ex)
            {
                LogError?.Invoke("{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Ensures that the <see cref="_listeners"/> for the given event id is not null.
        /// </summary>
        /// <param name="eventId"></param>
        private void EnsureListenerForEventIdIsNotNull(byte eventId)
        {
            _listeners[eventId] ??= new();
        }
    }
}