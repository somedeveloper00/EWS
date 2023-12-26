using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using EWS.Interfaces;

namespace EWS
{
    /// <summary>
    /// Represents a client for the EWS protocol.
    /// </summary>
    public class EwsClient
    {
        /// <summary>
        /// The underlying TCP client.
        /// </summary>
        private TcpClient _client;

        /// <summary>
        /// The thread that runs the client loop.
        /// </summary>
        private Thread _thread;

        /// <summary>
        /// The size of the buffer used to read data from the stream.
        /// </summary>
        private readonly int _bufferSize;

        /// <summary>
        /// The listeners for each event id.
        /// </summary>
        private readonly List<IEwsEventListener>[] _listeners = new List<IEwsEventListener>[byte.MaxValue];

        /// <summary>
        /// The byte that marks the end of a message.
        /// </summary>
        private const byte EndOfMessageByte = 0x00;

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

        /// <summary>
        /// uses the given client to create a new EwsClient.
        /// </summary>
        internal EwsClient(TcpClient client, int bufferSize = 1024)
        {
            _client = client;
            _bufferSize = bufferSize;
            _thread = new Thread(ClientLoop);
            _thread.Start();
        }

        /// <summary>
        /// Creates a new EwsClient.
        /// </summary>
        public EwsClient(int bufferSize = 1024)
        {
            _bufferSize = bufferSize;
        }

        /// <summary>
        /// Connects to the given host and port. Use events <see cref="Connected"/> and <see cref="Disconnected"/> 
        /// to listen for the result. When <see cref="IsConnected"/> is true, you don't need to use this.
        /// </summary>
        public void Connect(string hostName, int port)
        {
            if (IsConnected())
            {
                LogError?.Invoke("client already connected");
                return;
            }
            _thread = new Thread(() => ConnectAndClientLoop(hostName, port));
            _thread.Start();
        }

        /// <summary>
        /// Closes the connection. Use event <see cref="Disconnected"/> to listen for the result. When 
        /// <see cref="IsConnected"/> is false, you don't need to use this.
        /// </summary>
        public void Close()
        {
            if (!IsConnected())
            {
                LogError?.Invoke("client is not connected");
                return;
            }
            _thread?.Abort();
        }

        /// <summary>
        /// Returns true if the client is connected.
        /// </summary>
        public bool IsConnected()
        {
            return _thread is not null && _thread.IsAlive && _client.Connected;
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

            _client.GetStream().WriteAsync(data.ToArray(), 0, data.Count);
        }

        /// <summary>
        /// Blocks the thread, connecting the client and running the <see cref="ClientLoop"/> on this thread. 
        /// (Should NOT be called from the main thread)
        /// </summary>
        private void ConnectAndClientLoop(string uri, int port)
        {
            _client = new TcpClient();
            try
            {
                _client.Connect(uri, port);
                _client.GetStream().Write(EwsUtilities.EwsSecret);
            }
            catch (Exception ex)
            {
                LogError?.Exception(ex);
                return;
            }
            Connected?.Invoke();
            ClientLoop();
        }

        /// <summary>
        /// Runs the client loop. (Should NOT be called from the main thread)
        /// </summary>
        private void ClientLoop()
        {
            try
            {
                Span<byte> buffer = stackalloc byte[_bufferSize];
                List<byte> data = new(2048);
                var stream = _client.GetStream();

                while (true)
                {
                    if (!_client.Connected)
                    {
                        return;
                    }

                    var count = stream.Read(buffer);

                    // hook
                    streamManip?.OnRead(buffer, count);

                    if (count == 0)
                    {
                        _client.Close();
                        _client.Dispose();
                        Disconnected?.Invoke();
                    }

                    for (int i = 0; i < count; i++)
                    {
                        data.Add(buffer[i]);
                    }

                    if (data.Count > 0 && data[^1] == EndOfMessageByte)
                    {
                        HandleMessage(data);
                        data.Clear();
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _client.Close();
                _client.Dispose();
                Disconnected?.Invoke();
            }
            catch (Exception ex)
            {
                LogError?.Invoke("{0}\n{1}", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Handles the given message. Calls appropriate <see cref="_listeners"/>
        /// </summary>
        private void HandleMessage(List<byte> data)
        {
            byte eventId = data[0];
            var message = data.Skip(1).Take(data.Count - 1).ToArray();

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