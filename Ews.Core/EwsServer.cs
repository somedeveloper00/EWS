using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ews.Core
{
    public delegate void NewClientConnectedDelegate(EwsClient client);

    /// <summary>
    /// Represents a server for the EWS protocol.
    /// </summary>
    public sealed class EwsServer
    {
        /// <summary>
        /// The interval between reconnect attempts.
        /// </summary>
        public TimeSpan reconnectInterval = TimeSpan.FromSeconds(1);

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
        public readonly EndPoint endpoint;

        /// <summary>
        /// The cancellation token source that is used for cancelling the task that listens for clients.
        /// </summary>
        private CancellationTokenSource _cts;

        /// <summary>
        /// The socket that is used for listening for clients.
        /// </summary>
        private Socket _socket;

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
        public EwsServer(IPAddressEnum ipAddress, int port, ProtocolType protocol = ProtocolType.Tcp, bool keepAlive = true)
        {
            endpoint = new IPEndPoint(ipAddress.ToIpAddress(), port);
            this.protocol = protocol;
            this.keepAlive = keepAlive;
        }

        /// <summary>
        /// Returns true if the server is running.
        /// </summary>
        public bool IsListening() => _cts?.IsCancellationRequested == false && _socket?.IsBound == true;

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
                _socket = new(AddressFamily.InterNetwork, SocketType.Stream, protocol);

                if (keepAlive)
                {
                    _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                }

                _socket.Bind(endpoint);
                _socket.Listen(10);
                try
                {
                    ServerStartedListening?.Invoke();
                }
                catch (Exception ex)
                {
                    LogError?.Exception(ex);
                }
                _cts?.Cancel();
                _cts = new();
                StartCommunicationLoop(_cts.Token);
            }
            catch (Exception ex)
            {
                LogError?.Invoke($"could not connect to {endpoint}. {ex.Message}");
            }
        }

        public void Stop()
        {
            if (!IsListening())
            {
                return;
            }
            StopCommunicationLoop();
        }

        /// <summary>
        /// stops and disposes the <see cref="_socket"/> and restarts it.
        /// </summary>
        private async Task ReconnectAsync()
        {
            StopCommunicationLoop();
            await Task.Delay(reconnectInterval);
            Listen();
        }

        /// <summary>
        /// Starts the socket communication loop.
        /// </summary>
        private void StartCommunicationLoop(CancellationToken ct)
        {
            Task.Run(async () =>
            {
                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        var client = await _socket.AcceptAsync();

                        // handle client
                        _ = Task.Run(async () =>
                        {
                            // check if client is an EWS client
                            var stream = new NetworkStream(client);
                            var buffer = new byte[EwsUtilities.EwsSecret.Length];
                            int _ = await stream.ReadAsync(buffer, ct);
                            if (buffer.SequenceEqual(EwsUtilities.EwsSecret))
                            {
                                // accept
                                await client.SendAsync(EwsUtilities.SecretAccepted, SocketFlags.None);

                                // connect
                                var ewsClient = new EwsClient(client, keepAlive);

                                // keeping track of connected clients
                                ServerClosed += RemoveClient;
                                ewsClient.Disconnected += () => ServerClosed -= RemoveClient;
                                ewsClient.Connected += () => ServerClosed += RemoveClient;

                                NewClientConnected?.Invoke(ewsClient);

                                void RemoveClient()
                                {
                                    try
                                    {
                                        ewsClient.Disconnect();
                                    }
                                    catch (Exception ex)
                                    {
                                        LogError?.Exception(ex);
                                    }
                                }
                            }
                            else
                            {
                                // client secrets were wrong
                                client.Close();
                            }

                        }).ConfigureAwait(true);
                    }
                    catch (Exception ex)
                    {
                        if (_cts.IsCancellationRequested)
                        {
                            break;
                        }
                        LogError?.Exception(ex);
                        await ReconnectAsync();
                    }
                }
                _socket.Dispose();
                ServerClosed?.Invoke();
            }, ct);
        }

        /// <summary>
        /// Stops the socket communication loop. (the task from <see cref="StartCommunicationLoop"/> will be 
        /// cancelled)
        /// </summary>
        private void StopCommunicationLoop()
        {
            _cts?.Cancel();
            if (_socket?.Connected == true)
            {
                // necessary to stop the socket from listening
                _socket.Shutdown(SocketShutdown.Both);
            }
            _socket?.Close();
        }
    }
}