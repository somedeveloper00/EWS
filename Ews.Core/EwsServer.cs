using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Ews.Core
{
    public delegate void NewClientConnectedDelegate(EwsClient client);

    /// <summary>
    /// Represents a server for the EWS protocol.
    /// </summary>
    public sealed class EwsServer
    {
        /// <summary>
        /// The protocol that the server uses.
        /// </summary>
        public readonly ProtocolType protocol;

        /// <summary>
        /// If true, the server will keep the connection alive.
        /// </summary>
        public readonly bool keepAlive;

        /// <summary>
        /// The endpoint that the server listens on.
        /// </summary>
        public readonly IPEndPoint endpoint;

        /// <summary>
        /// The maximum number of 
        /// </summary>
        public readonly int socketBacklog;

        /// <summary>
        /// The maximum length of the pending connections queue.
        /// </summary>
        private Thread _socketThread;

        /// <summary>
        /// Raised when a new client connects. (the new <see cref="EwsClient"/> does not need to manually connect, 
        /// as its already connected)
        /// </summary>
        public event NewClientConnectedDelegate NewClientConnected;

        /// <summary>
        /// Raised when the server starts listening.
        /// </summary>
        public event EmptyDelegate ServerStartedListening;

        /// <summary>
        /// Raised when the server closes.
        /// </summary>
        public event EmptyDelegate ServerClosed;

        /// <summary>
        /// Raised when an error occurs.
        /// </summary>
        public event LogErrorDelegate LogError;

        /// <summary>
        /// Creates a new server that listens on the given port.
        /// </summary>
        public EwsServer(IPAddressEnum ipAddress, int port, ProtocolType protocol = ProtocolType.Tcp, bool keepAlive = true, int socketBacklog = 10)
        {
            endpoint = new IPEndPoint(ipAddress.ToIpAddress(), port);
            this.protocol = protocol;
            this.keepAlive = keepAlive;
            this.socketBacklog = socketBacklog;
        }

        /// <summary>
        /// Returns true if the server is running.
        /// </summary>
        public bool IsListening() => _socketThread != null;

        /// <summary>
        /// Starts listening for clients. Check <see cref="ServerStartedListening"/> for the result;
        /// </summary>
        public void Listen()
        {
            if (IsListening())
            {
                return;
            }

            try
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocol);

                if (keepAlive)
                {
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                }

                socket.Bind(endpoint);
                socket.Listen(socketBacklog);
                _socketThread = StartCommunicationThread(socket);
                OnServerStartedListening();
            }
            catch (Exception ex)
            {
                OnLogException(ex);
            }
        }

        public void Stop()
        {
            if (!IsListening())
            {
                return;
            }
            StopCommunicationThread();
        }

        /// <summary>
        /// Starts the socket communication loop.
        /// </summary>
        private Thread StartCommunicationThread(Socket socket)
        {
            var thread = new Thread(() =>
            {
                HashSet<Thread> threads = new(32);
                while (true)
                {
                    try
                    {
                        var client = socket.Accept();
                        Thread newThread = new(() => HandleClient(client));
                        newThread.Start();
                        threads.Add(newThread);
                    }
                    catch (ThreadAbortException)
                    {
                        DisposeEverything();
                        return;
                    }
                    catch (Exception ex)
                    {
                        OnLogException(ex); ;
                        DisposeEverything();
                        return;
                    }
                }

                void DisposeEverything()
                {
                    // socket?.Disconnect(false);
                    socket?.Dispose();
                    OnServerClosed();
                    foreach (var thread in threads.Where(t => t?.IsAlive == true))
                        thread.Abort();
                }
            });
            thread.Start();
            return thread;

            void HandleClient(Socket client)
            {
                // check if client is an EWS client
                var stream = new NetworkStream(client);
                Span<byte> buffer = stackalloc byte[EwsUtilities.EwsSecret.Length];
                int _ = stream.Read(buffer);
                if (buffer.SequenceEqual(EwsUtilities.EwsSecret))
                {
                    // accept
                    client.Send(EwsUtilities.SecretAccepted, SocketFlags.None);

                    // connect
                    var ewsClient = new EwsClient(client, keepAlive);

                    // keeping track of connected clients
                    ServerClosed += RemoveClient;
                    ewsClient.Disconnected += () => ServerClosed -= RemoveClient;
                    ewsClient.Connected += () => ServerClosed += RemoveClient;

                    OnNewClientConnected(ewsClient);

                    void RemoveClient()
                    {
                        try
                        {
                            ewsClient.Disconnect();
                        }
                        catch (Exception ex)
                        {
                            OnLogException(ex);
                        }
                    }
                }
                else
                {
                    // client secrets were wrong
                    client.Close();
                }
            }
        }

        private void OnServerClosed()
        {
            try
            {
                ServerClosed?.Invoke();
            }
            catch (Exception ex)
            {
                OnLogException(ex);
            }
        }

        /// <summary>
        /// Stops the socket communication loop. (the task from <see cref="StartCommunicationThread"/> will be 
        /// cancelled)
        /// </summary>
        private void StopCommunicationThread()
        {
            _socketThread?.Abort();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnLogException(Exception ex)
        {
            try
            {
                LogError?.Exception(ex);
            }
            catch { }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void OnNewClientConnected(EwsClient ewsClient)
        {
            try
            {
                NewClientConnected?.Invoke(ewsClient);
            }
            catch (Exception ex)
            {
                OnLogException(ex);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnServerStartedListening()
        {
            try
            {
                ServerStartedListening?.Invoke();
            }
            catch (Exception ex)
            {
                OnLogException(ex);
            }
        }
    }
}