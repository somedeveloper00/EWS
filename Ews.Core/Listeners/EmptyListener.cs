using System;
using Ews.Core.Interfaces;

namespace Ews.Core.Listeners
{
    /// <summary>
    /// Simple listener that invokes when an event is received. It wont pass any data.
    /// </summary>
    public sealed class EmptyListener : IEwsEventListener
    {
        private readonly Action received;

        public EmptyListener(Action received)
        {
            this.received = received;
        }

        public void Process(EwsClient client, byte[] bytes)
        {
            received();
        }
    }
}