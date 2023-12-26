using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using EWS;

public delegate void NewClientConnectedDelegate(EwsClient client);

/// <summary>
/// Represents a server for the EWS protocol.
/// </summary>
public class EwsServer
{
    /// <summary>
    /// The thread that runs the server loop.
    /// </summary>
    private Thread _thread;

    /// <summary>
    /// The port to listen on.
    /// </summary>
    private readonly int _port;

    /// <summary>
    /// The IP address to listen on.
    /// </summary>
    private readonly IPAddress _ipAddress;

    /// <summary>
    /// Raised when a new client connects. (the new <see cref="EwsClient"/> does not need to manually connect, 
    /// as its already connected)
    /// </summary>
    public event NewClientConnectedDelegate NewClientConnected;

    /// <summary>
    /// Raised when the server starts. (not just the first time)
    /// </summary>
    public event EmptyDelegate ServerStarted;

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
    public EwsServer(IPAddressEnum iPAddress, int port)
    {
        _ipAddress = iPAddress.ToIpAddress();
        _port = port;
    }

    /// <summary>
    /// Starts the server.
    /// </summary>
    public void Start()
    {
        if (IsRunning())
        {
            LogError?.Invoke($"server is already running.");
            return;
        }
        _thread = new Thread(ServerLoop);
        _thread.Start();
    }

    /// <summary>
    /// Closes the server.
    /// </summary>
    public void Close()
    {
        if (!IsRunning())
        {
            LogError?.Invoke("server is not running.");
            return;
        }
        _thread?.Abort();
    }

    /// <summary>
    /// Returns true if the server is running.
    /// </summary>
    public bool IsRunning() => _thread is not null && _thread.IsAlive;

    /// <summary>
    /// The server loop. (should NOT be called from the main thread)
    /// </summary>
    private void ServerLoop()
    {
        var listener = new TcpListener(_ipAddress, _port);
        try
        {
            try
            {
                listener.Start();
            }
            catch (SocketException ex)
            {
                LogError?.Exception(ex);
                return;
            }

            ServerStarted?.Invoke();

            while (true)
            {
                var client = listener.AcceptTcpClient();
                HandleClient(client);
            }
        }
        catch (ThreadAbortException)
        {
            listener?.Stop();
            ServerClosed?.Invoke();
        }
        catch (Exception ex)
        {
            // unexpected
            LogError?.Exception(ex);
        }
    }

    /// <summary>
    /// Handles a client. (should NOT be called from the main thread)
    /// </summary>
    private void HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        Span<byte> bytes = stackalloc byte[EwsUtilities.EwsSecretLength];
        int c = stream.Read(bytes);
        if (c == EwsUtilities.EwsSecretLength && bytes.SequenceEqual(EwsUtilities.EwsSecret))
        {
            // connect
            var ewsClient = new EwsClient(client);

            // keeping track of connected clients
            ServerClosed += RemoveClient;
            ewsClient.Disconnected += () => ServerClosed -= RemoveClient;
            ewsClient.Connected += () => ServerClosed += RemoveClient;


            try
            {
                NewClientConnected?.Invoke(ewsClient);
            }
            catch (Exception ex)
            {
                LogError?.Exception(ex);
            }

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
            LogError?.Invoke("client secrets are wrong. closing connection.");
            client.Close();
        }
    }
}