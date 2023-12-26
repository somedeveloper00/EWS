using System;
using EWS.Extensions;
using EWS.Interfaces;

namespace EWS.Listeners
{
    /// <summary>
    /// Simple listener that invokes an action with the received object. Converts the bytes to the 
    /// object.
    /// </summary>
    public class ObjectListener<T> : IEwsEventListener
        where T : unmanaged
    {
        private readonly Action<T> objectReceived;
        private readonly Action<Exception> onSerializationError;

        public ObjectListener(Action<T> objectReceived, Action<Exception> onSerializationError = null)
        {
            this.objectReceived = objectReceived ?? throw new ArgumentNullException(nameof(objectReceived));
            this.onSerializationError = onSerializationError;
        }

        public void Process(EwsClient client, byte[] bytes)
        {
            try
            {
                var obj = bytes.FromNetworkByteArray<T>();
                objectReceived(obj);
            }
            catch (Exception ex)
            {
                if (onSerializationError is not null)
                    onSerializationError(ex);
                else 
                    throw ex;
            }
        }
    }
}