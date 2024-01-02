using System.Threading.Tasks;
using UnityEngine;

namespace EWS.Unity
{
    /// <summary>
    /// A component that starts an EWS client.
    /// </summary>
    public class EwsClientComponent : MonoBehaviour
    {
        [SerializeField] private string address;
        [SerializeField] private int port;
        [SerializeField] private Vector3 val;
        private EwsClient _client;

        [ContextMenu(nameof(ConnectAsync))]
        private async Task ConnectAsync()
        {
            if (_client?.IsConnected() ?? false)
            {
                Debug.LogError("client is connected");
                return;
            }
            _client = new(address, port);
            _client.LogError += LogError;
            _client.Connected += () => Debug.Log("client connecetd!");
            _client.Disconnected += () => Debug.Log("client disconnected!");

            _client.AddListener(0x02, new ByteArrayListener((client, bytes) =>
            {
                Debug.Log($"Received bytes: {bytes.Length}\n[{string.Join(", ", bytes)}]");
            }));

            await _client.ConnectAsync();
        }

        [ContextMenu(nameof(Send))]
        private void Send()
        {
            _client.SendUnityJson(2, val);
        }

        [ContextMenu(nameof(Send111))]
        private void Send111()
        {
            for (int i = 0; i < 1111; i++)
            {
                val.z += 1;
                _client.SendUnityJson(2, val);
                Debug.LogFormat("sent {0}", val);
            }
        }

        [ContextMenu(nameof(Close))]
        private void Close()
        {
            _client?.Disconnect();
        }

        private void LogError(string msg, object[] args)
        {
            Debug.LogErrorFormat(msg, args);
        }
    }
}