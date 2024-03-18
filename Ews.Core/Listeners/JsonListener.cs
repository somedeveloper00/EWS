using System;
using System.Runtime.Serialization;
using System.Text;
using Ews.Core.Extensions;
using Ews.Core.Interfaces;

namespace Ews.Core.Listeners
{
    /// <summary>
    /// Simple listener that invokes an action with the received object.
    /// </summary>
    /// <typeparam name="T">type of the object expecetd to receive</typeparam>
    public sealed class JsonListener<T> : IEwsEventListener
    {
        private readonly IEwsJsonSerializer<T> _serializer;
        private readonly Action<T> _onObjectReceived;
        private readonly Action<SerializationException> _onSerializationError;

        public JsonListener(IEwsJsonSerializer<T> serializer, Action<T> onObjectReceived,
            Action<SerializationException> onSerializationError = null)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _onObjectReceived = onObjectReceived ?? throw new ArgumentNullException(nameof(onObjectReceived));
            _onSerializationError = onSerializationError;
        }

        public void Process(EwsClient client, byte[] bytes)
        {
            try
            {
                string msg = Encoding.UTF8.GetString(bytes);
                T obj = _serializer.Deserialize(msg);
                _onObjectReceived(obj);
            }
            catch (SerializationException e)
            {
                _onSerializationError?.Invoke(e);
            }
        }
    }
}