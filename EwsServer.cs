using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using EWS;

public delegate void NewClientConnectedDelegate(EwsClient client);

/// <summary>
/// Represents a server for the EWS protocol.
/// </summary>
public class EwsServer
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
    /// Raised when the server starts. (not just the first time)
    /// </summary>
    public event EmptyDelegate ServerConnected;

    /// <summary>
    /// Raised when the server closes. (not just the first time)
    /// </summary>
    public event EmptyDelegate ServerClosed;

    /// <summary>
    /// Raised when an error occurs.
    /// </summary>
    public event LogErrorDelegate LogError;

    /// <summary>
    /// Creates a new server that listens on the given port.
    /// </summary>
    public EwsServer(IPAddressEnum ipAddress, int port, ProtocolType protocol = ProtocolType.Tcp,
        bool keepAlive = true)
    {
        endpoint = new IPEndPoint(ipAddress.ToIpAddress(), port);
        this.protocol = protocol;
        this.keepAlive = keepAlive;
    }

    /// <summary>
    /// Returns true if the server is running.
    /// </summary>
    public bool IsRunning() => _cts?.IsCancellationRequested == false && _socket?.Connected == true;

    /// <summary>
    /// Starts listening for clients. Check <see cref="ServerConnected"/> for the result;
    /// </summary>
    public void Listen()
    {
        try
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocol);

            if (keepAlive)
            {
                _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            }

            _socket.Bind(endpoint);
            _socket.Listen(10);
            try
            {
                ServerConnected?.Invoke();
            }
            catch (Exception ex)
            {
                LogError?.Exception(ex);
            }
            StartCommunicationLoop();
        }
        catch (Exception ex)
        {
            LogError?.Invoke($"could not connect to {endpoint}. {ex.Message}");
            return;
        }
    }

    public void Stop()
    {
        if (IsRunning())
        {
            StopCommunicationLoop();
            _socket.Dispose();
        }
    }

    /// <summary>
    /// stops and disposes the <see cref="_socket"/> and restarts it.
    /// </summary>
    private async Task ReconnectAsync()
    {
        StopCommunicationLoop();
        _socket.Dispose();
        await Task.Delay(reconnectInterval);
        try
        {
            await _socket.ConnectAsync(_socket.RemoteEndPoint);
            try
            {
                ServerConnected?.Invoke();
            }
            catch (Exception ex)
            {
                LogError?.Exception(ex);
            }
            StartCommunicationLoop();
        }
        catch (Exception ex)
        {
            LogError?.Invoke($"could not connect to {endpoint}. {ex.Message}");
            await ReconnectAsync();
        }
    }

    /// <summary>
    /// Starts the socket communication loop.
    /// </summary>
    private void StartCommunicationLoop()
    {
        _cts = new CancellationTokenSource();
        Task.Run(async () =>
        {
            while (!_cts.IsCancellationRequested)
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
                        int c = await stream.ReadAsync(buffer, _cts.Token);
                        if (c == EwsUtilities.EwsSecret.Length && buffer.SequenceEqual(EwsUtilities.EwsSecret, 0, c))
                        {
                            // accept
                            await client.SendAsync(EwsUtilities.EwsSecret, SocketFlags.None);

                            // connect
                            var ewsClient = new EwsClient(client);

                            // keeping track of connected clients
                            ServerClosed += RemoveClient;
                            ewsClient.Disconnected += () => ServerClosed -= RemoveClient;
                            ewsClient.Connected += () => ServerClosed += RemoveClient;

                            void RemoveClient()
                            {
                                try
                                {
                                    ewsClient.Close();
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
                    LogError?.Exception(ex);
                    await ReconnectAsync();
                }
            }
            ServerClosed?.Invoke();
        });
    }

    /// <summary>
    /// Stops the socket communication loop. (the task from <see cref="StartCommunicationLoop"/> will be 
    /// cancelled)
    /// </summary>
    private void StopCommunicationLoop()
    {
        _cts?.Cancel();
    }
}