using System;
using Ews.Core.Interfaces;

namespace Ews.Core.Listeners
{
    /// <summary>
    /// Simple listener that invokes an action with the received byte array.
    /// </summary>
    public sealed class ByteArrayListener : IEwsEventListener
    {
        private readonly Action<EwsClient, byte[]> _action;

        public ByteArrayListener(Action<EwsClient, byte[]> action)
        {
            _action = action;
        }

        public void Process(EwsClient client, byte[] bytes)
        {
            _action?.Invoke(client, bytes);
        }
    }
}