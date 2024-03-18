using System;
using System.Text;
using Ews.Core.Interfaces;

namespace Ews.Core.Listeners
{
    /// <summary>
    /// Simple listener that invokes an action with the received string. (using utf8 encoding)
    /// </summary>
    public sealed class StringListener : IEwsEventListener
    {
        private readonly Action<string> received;
        private readonly Action<Exception> onError;

        public StringListener(Action<string> received, Action<Exception> onError = null)
        {
            this.received = received ?? throw new ArgumentNullException(nameof(received));
            this.onError = onError;
        }

        public void Process(EwsClient client, byte[] bytes)
        {
            try
            {
                var str = Encoding.UTF8.GetString(bytes);
                received(str);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }
    }
}