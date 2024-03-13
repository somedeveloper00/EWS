using System;
using EWS.Extensions;
using UnityEngine;

namespace EWS.Unity
{
    public static class EwsClientExtensions
    {
        /// <summary>
        /// Sends a json serialized by Unity's JsonUtility.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="client">The EwsClient instance.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="obj">The object to be serialized.</param>
        public static void SendUnityJson<T>(this EwsClient client, byte eventId, T obj)
        {
            client.SendJson(eventId, obj, EwsSerializerJsonUtility<T>.Cached);
        }

        /// <summary>
        /// Adds a Unity JSON listener to the EwsClient for the specified event ID.
        /// </summary>
        /// <typeparam name="T">The type of data received from the listener.</typeparam>
        /// <param name="client">The EwsClient instance.</param>
        /// <param name="eventId">The event ID to listen for.</param>
        /// <param name="received">The action to be executed when data is received.</param>
        /// <param name="error">The action to be executed when an error occurs.</param>
        public static void AddUnityJsonListener<T>(this EwsClient client, byte eventId, Action<T> received,
            Action<Exception> error = null)
        {
            client.AddJsonListener(eventId, EwsSerializerJsonUtility<T>.Cached, received, error);
        }
    }

    /// <summary>
    /// Provides serialization and deserialization functionality using Unity's JsonUtility.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize or deserialize.</typeparam>
    public class EwsSerializerJsonUtility<T> : IEwsJsonSerializer<T>
    {
        /// <summary>
        /// The cached instance of the EwsJsonUtilitySerializer. (not pretty printed)
        /// </summary>
        public static readonly EwsSerializerJsonUtility<T> Cached = new(false);

        /// <summary>
        /// The cached instance of the EwsJsonUtilitySerializer. (pretty printed)
        /// </summary>
        public static readonly EwsSerializerJsonUtility<T> CachedPretty = new(true);

        private readonly bool prettyPrint;

        /// <summary>
        /// Creates a new instance of the EwsJsonUtilitySerializer.
        /// </summary>
        /// <param name="prettyPrint">Whether to pretty print the json.</param>
        public EwsSerializerJsonUtility(bool prettyPrint)
        {
            this.prettyPrint = prettyPrint;
        }

        /// <summary>
        /// Deserializes the specified JSON string into an object of type T.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Populates the specified object with values from the JSON string.
        /// </summary>
        /// <param name="json">The JSON string to populate the object from.</param>
        /// <param name="obj">The object to populate.</param>
        public void Populate(string json, T obj)
        {
            JsonUtility.FromJsonOverwrite(json, obj);
        }

        /// <summary>
        /// Serializes the specified object into a JSON string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The serialized JSON string.</returns>
        public string Serialize(T obj)
        {
            return JsonUtility.ToJson(obj, prettyPrint);
        }
    }
}