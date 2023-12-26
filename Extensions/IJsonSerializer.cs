namespace EWS
{
    /// <summary>
    /// Represents a serializer for json objects.
    /// </summary>
    public interface IEwsJsonSerializer<T>
    {
        /// <summary>
        /// Serializes the object to a json string.
        /// </summary>
        /// <exception cref="SerializationException"> if the object cannot be serialized</exception>
        string Serialize(T obj);

        /// <summary>
        /// Deserializes the json string to an object.
        /// </summary>
        /// <exception cref="SerializationException"> if the json string is invalid</exception>
        T Deserialize(string json);

        /// <summary>
        /// Populates the object with the json string.
        /// </summary>
        /// <exception cref="SerializationException"> if the json string is invalid</exception>
        void Populate(string json, T obj);
    }
}