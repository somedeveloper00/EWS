using System;
using UnityEngine;

namespace EWS.Unity
{
    /// <summary>
    /// A component that starts an EWS server.
    /// </summary>
    public class EwsServerComponent : MonoBehaviour
    {
        [Tooltip("The ip address to listen on.")]
        [SerializeField] IPAddressEnum ipAddress;

        [Tooltip("The port to listen on.")]
        [SerializeField] int port;

        [SerializeField] Vector3 val;
        private EwsServer server;

        void Start()
        {
            StartServer();
        }

        [ContextMenu(nameof(StartServer))]
        public void StartServer()
        {
            if (server?.IsRunning() ?? false)
            {
                server.Stop();
            }
            server = new(ipAddress, port);
            server.ServerClosed += () => Debug.Log("server closed!");
            server.ServerConnected += () => Debug.Log("server started!");
            server.LogError += LogError;
            server.NewClientConnected += NewClientConnected;
            server.Listen();
        }

        private void NewClientConnected(EwsClient client)
        {
            client.AddUnityJsonListener<Vector3>(2, v => val = v);
        }

        [ContextMenu(nameof(CloseServer))]
        public void CloseServer()
        {
            server?.Stop();
        }

        private void LogError(string msg, object[] args)
        {
            Debug.LogErrorFormat(msg, args);
        }

        void OnDestroy()
        {
            CloseServer();
        }
    }
}