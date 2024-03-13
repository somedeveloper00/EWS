namespace EWS.Extensions
{
    /// <summary>
    /// Represents a serializer for json objects.
    /// </summary>
    public interface IEwsJsonSerializer<T>
    {
        /// <summary>
        /// Serializes the object to a json string.
        /// </summary>
        string Serialize(T obj);

        /// <summary>
        /// Deserializes the json string to an object.
        /// </summary>
        T Deserialize(string json);

        /// <summary>
        /// Populates the object with the json string.
        /// </summary>
        void Populate(string json, T obj);
    }
}