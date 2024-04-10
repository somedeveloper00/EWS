using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Ews.Core.Interfaces;

namespace Ews.Core
{
    /// <summary>
    /// Represents a client for the EWS protocol.
    /// </summary>
    public sealed class EwsClient
    {
        /// <summary>
        /// The interval between reconnect attempts.
        /// </summary>
        public TimeSpan reconnectInterval = TimeSpan.FromSeconds(2);

        /// <summary>
        /// The timeout for connecting to the server.
        /// </summary>
        public TimeSpan connectTimeout = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Intervals (in seconds) to send keep alive messages to the other server.
        /// </summary>
        public TimeSpan keepAliveIntervals = TimeSpan.FromSeconds(2);

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
        /// maximum number of tries to connect to the server.
        /// </summary>
        private readonly int _maxConnectRetries;

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
        /// Raised when attempting a reconnect
        /// </summary>
        public event EmptyDelegate ReconnectAttempted;

        /// <summary>
        /// The encryption algorithm used for sending and receiving data. (null for no encryption)
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

        public IConnectionEventsPreprocess connectionEventsPreprocess;

        private CancellationTokenSource _clientLoopCts;

        private CancellationTokenSource _connectCts;

        private readonly bool _keepAlive;

        /// <summary>
        /// uses the given client to create a new EwsClient.
        /// </summary>
        internal EwsClient(Socket socket, bool keepAlive, int bufferSize = 1024, int maxConnectRetries = 0)
        {
            _socket = socket;
            _bufferSize = bufferSize;
            _endpoint = socket.RemoteEndPoint;
            _protocol = socket.ProtocolType;
            _keepAlive = keepAlive;
            _maxConnectRetries = maxConnectRetries;
            _clientLoopCts = new();
            StartCommunicationLoop(_clientLoopCts.Token);
        }

        /// <summary>
        /// Creates a new EwsClient.
        /// </summary>
        public EwsClient(string host, int port, ProtocolType protocol = ProtocolType.Tcp,
            int bufferSize = 1024, bool keepAlive = true, int maxConnectRetries = 5)
        {
            _bufferSize = bufferSize;
            _protocol = protocol;
            _keepAlive = keepAlive;
            _endpoint = new IPEndPoint(IPAddress.Parse(host), port);
            _maxConnectRetries = maxConnectRetries;
        }

        /// <summary>
        /// Connects to the given host and port. Use events <see cref="Connected"/> and <see cref="Disconnected"/> 
        /// to listen for the result. When <see cref="IsConnected"/> is true, you don't need to use this.
        /// </summary>
        public async Task<bool> ConnectAsync()
        {
            _connectCts?.Cancel();
            _connectCts = new();
            await InternalConnectAsync(_maxConnectRetries);
            return IsConnected();
        }

        /// <summary>
        /// Closes the connection. Use event <see cref="Disconnected"/> to listen for the result. When 
        /// <see cref="IsConnected"/> is false, you don't need to use this.
        /// </summary>
        public void Disconnect()
        {
            StopCommunicationLoop();
        }

        /// <summary>
        /// Returns true if the client is connected.
        /// </summary>
        public bool IsConnected()
        {
            return _clientLoopCts?.IsCancellationRequested == false && _socket?.Connected == true;
        }

        /// <summary>
        /// Returns the remote endpoint of the internal socket
        /// </summary>
        public EndPoint GetRemoteEndPoint() => _socket?.RemoteEndPoint;

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
                Error("client is not connected");
                return;
            }
            if (eventId == 0x00)
            {
                Error("cannot send message with event id 0x00");
                return;
            }

            // hook
            if (encryption is not null)
            {
                encryption.Encrypt(this, ref message, out var errorMessage);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Error(errorMessage);
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

            _socket.SendAsync(data.ToArray(), SocketFlags.None, _clientLoopCts.Token);
        }

        /// <summary>
        /// Connects to the given host and port. Use events <see cref="Connected"/> and <see cref="Disconnected"/>
        /// </summary>
        private async Task InternalConnectAsync(int remainingRetries)
        {
            if (IsConnected())
            {
                return;
            }

            _clientLoopCts?.Cancel();
            var timeoutCts = new CancellationTokenSource();

            try
            {
                // connect
                _socket = new(AddressFamily.InterNetwork, SocketType.Stream, _protocol);
                if (_keepAlive)
                {
                    _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                }
                timeoutCts = new();
                timeoutCts.CancelAfter(connectTimeout);
                await _socket.ConnectAsync(_endpoint).WithToken(timeoutCts.Token).WithToken(_connectCts.Token);

                // send EWS secret
                await _socket.SendAsync(EwsUtilities.EwsSecret, SocketFlags.None, _connectCts.Token);

                // listen for server's acceptance
                var buffers = new byte[EwsUtilities.SecretAccepted.Length];
                var c = await _socket.ReceiveAsync(buffers, SocketFlags.None, _connectCts.Token);

                if (buffers.SequenceEqual(EwsUtilities.SecretAccepted))
                {
                    // server has accepted connection
                    if (connectionEventsPreprocess is not null)
                        connectionEventsPreprocess.Connected(() => Connected?.Invoke());
                    else
                        Connected?.Invoke();

                    // start loop
                    _clientLoopCts?.Cancel();
                    _clientLoopCts = new();
                    StartCommunicationLoop(_clientLoopCts.Token);
                }
                else
                {
                    Error("server refused EWS connection");
                    // server has refused connection
                    Close();
                }
            }
            catch (Exception ex)
            {
                Close();

                if (_connectCts.IsCancellationRequested)
                {
                    return;
                }

                // log unexpected error
                if (!timeoutCts.IsCancellationRequested)
                {
                    ErrorException(ex);
                }

                // retry
                bool reconnectIntervalIsMoreThanZero = reconnectInterval > TimeSpan.Zero;
                if (reconnectIntervalIsMoreThanZero && remainingRetries > 0)
                {
                    await ReconnectAsync(--remainingRetries);
                }
            }
            return;

            void Close()
            {
                _socket?.Close();
                _socket?.Dispose();
            }
        }

        /// <summary>
        /// Sends the given message to the server. Use <see cref="IsConnected"/> to check if the
        /// </summary>
        private async Task ReconnectAsync(int remainingRetries)
        {
            if (connectionEventsPreprocess is not null)
                connectionEventsPreprocess.ReconnectAttempted(() => ReconnectAttempted?.Invoke());
            else
                ReconnectAttempted?.Invoke();
            StopCommunicationLoop();

            _connectCts = new();
            await Task.Delay(reconnectInterval, _connectCts.Token);
            await InternalConnectAsync(remainingRetries);
        }

        /// <summary>
        /// cancels thread which started from <see cref="StartCommunicationLoop"/> and closes <see cref="_socket"/>
        /// </summary>
        private void StopCommunicationLoop()
        {
            _connectCts?.Cancel();
            _clientLoopCts?.Cancel();
        }

        /// <summary>
        /// Stops the socket communication loop.
        /// </summary>
        private void StartCommunicationLoop(CancellationToken ct)
        {
            var mainLoopThread = new Thread(() =>
            {
                var buffer = new byte[_bufferSize];
                var data = new List<byte>(_bufferSize);

                // listen for messages
                while (true)
                {
                    try
                    {
                        var c = _socket.Receive(buffer, SocketFlags.None, out _);

                        // 0 length data happens when the connection is lost
                        if (c == 0)
                        {
                            Task.Run(() => ReconnectAsync(_maxConnectRetries), ct);
                            break;
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
                    catch (ThreadAbortException)
                    {
                        if (connectionEventsPreprocess is not null)
                            connectionEventsPreprocess.Disconnected(() => Disconnected?.Invoke());
                        else
                            Disconnected?.Invoke();

                        _socket?.Dispose();
                        break;
                    }
                    catch (Exception ex)
                    {
                        ErrorException(ex);
                    }
                }
            });

            var keepAliveThread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(keepAliveIntervals);
                    if (!IsConnected())
                    {
                        break;
                    }
                    try
                    {
                        int c = _socket.Send(new byte[] { 0x00 }, SocketFlags.None, out var errorCode);
                        if (c == 0) // its disconnected
                        {
                            throw new();
                        }
                    }
                    catch (ThreadAbortException)
                    {
                        // safe to ignore
                    }
                    catch (Exception)
                    {
                        // connection is lost. try to reconnect
                        StopCommunicationLoop();
                        ReconnectAsync(_maxConnectRetries).ConfigureAwait(false);
                    }
                }
            });

            ct.Register(() =>
            {
                mainLoopThread?.Abort();
                keepAliveThread?.Abort();
            });
            mainLoopThread.Start();
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
                    Error(errorMessage);
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
                Error("{0}\n{1}", ex.Message, ex.StackTrace);
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

        /// <summary>
        /// Raises <see cref="LogError"/> either through <see cref="connectionEventsPreprocess"/> or directoly
        /// </summary>
        private void Error(string message, params object[] args)
        {
            if (connectionEventsPreprocess is not null)
                connectionEventsPreprocess.OnError(() => LogError?.Invoke(message, args));
            else
                LogError?.Invoke(message, args);
        }

        /// <summary>
        /// Same as <see cref="Error"/>, only uses a nice format for <param name="exception"></param>.
        /// </summary>
        private void ErrorException(Exception exception)
        {
            if (connectionEventsPreprocess is not null)
                connectionEventsPreprocess.OnError(() =>
                    LogError?.Invoke("{0}:{1}\n{2}", exception.GetType().Name, exception.Message, exception.StackTrace));
            else
                LogError?.Invoke("{0}:{1}\n{2}", exception.GetType().Name, exception.Message, exception.StackTrace);
        }
    }
}