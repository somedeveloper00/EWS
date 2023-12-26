using System;
using System.Runtime.Serialization;
using System.Text;
using EWS.Extensions;
using EWS.Interfaces;
using EWS.Listeners;

namespace EWS
{
    /// <summary>
    /// contains extensions for the EwsClient class.
    /// </summary>
    public static class EwsClientExtensions
    {
        /// <summary>
        /// sends a string message to the server, with utf8 encoding.
        /// </summary>
        public static void SendString(this EwsClient client, byte eventId, string message)
        {
            client.Send(eventId, Encoding.UTF8.GetBytes(message));
        }

        /// <summary>
        /// sends a json object to the server form <paramref name="obj"/>, with utf8 encoding.
        /// </summary>
        public static void SendJson<T>(this EwsClient client, byte eventId, T obj, IEwsJsonSerializer<T> serializer)
        {
            client.SendString(eventId, serializer.Serialize(obj));
        }

        /// <summary>
        /// sends an object of the specified type <typeparamref name="T"/> to the server. 
        /// (using byte array marshalling,
        /// </summary>
        public static void SendObject<T>(this EwsClient client, byte eventId, T obj)
            where T : unmanaged
        {
            client.Send(eventId, obj.ToNetworkByteArray());
        }

        /// <summary>
        /// adds a listener that receives an object of the specified type <typeparamref name="T"/> from 
        /// the server. (using byte array marshalling, so the object must be unmanaged) 
        /// </summary>
        public static IEwsEventListener AddObjectListener<T>(this EwsClient client, byte eventId, Action<T> received,
            Action<Exception> error = null)
            where T : unmanaged
        {
            ObjectListener<T> listener = new(received, error);
            client.AddListener(eventId, listener);
            return listener;
        }

        /// <summary>
        /// adds a listener that receives no object and just invokes the specified action when an event is received.
        /// </summary>
        public static IEwsEventListener AddEmptyListener(this EwsClient client, byte eventId, Action received)
        {
            EmptyListener listener = new(received);
            client.AddListener(eventId, listener);
            return listener;
        }

        /// <summary>
        /// adds a listener that receives a string from the server, with utf8 encoding.
        /// </summary>
        public static IEwsEventListener AddStringListener(this EwsClient client, byte eventId,
            Action<string> received, Action<Exception> error = null)
        {
            StringListener listener = new(received, error);
            client.AddListener(eventId, listener);
            return listener;
        }

        /// <summary>
        /// adds a listener that receives a json for the specified type <typeparamref name="T"/> from the server.
        /// </summary>
        public static IEwsEventListener AddJsonListener<T>(this EwsClient client, byte eventId,
            IEwsJsonSerializer<T> serializer, Action<T> received, Action<SerializationException> error = null)
        {
            JsonListener<T> listener = new(serializer, received, error);
            client.AddListener(eventId, listener);
            return listener;
        }
    }
}