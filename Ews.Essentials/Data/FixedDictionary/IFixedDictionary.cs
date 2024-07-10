namespace Ews.Essentials.Data
{
    /// <summary>
    /// Fixed sized dictionary
    /// </summary>
    public interface IFixedDictionary<TKey, TVal>
        where TKey : unmanaged
        where TVal : unmanaged
    {
        /// <summary>
        /// Get a ref to the element at the specified key. This will use the internal 
        /// hash table to find the value by reference. This won't raise exception if the 
        /// entry for the key is non-existent, instead it will create it.
        /// </summary>
        ref TVal this[TKey key] { get; }

        /// <summary>
        /// Count of the entries. This is an expensive call.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Count of the bucket.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        /// Add a new entry
        /// </summary>
        bool Add(TKey key, TVal val);

        /// <summary>
        /// Remove an entry by key.
        /// </summary>
        bool Remove(TKey key);

        /// <summary>
        /// Clears all data
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns whether the internal hashtable contains the key
        /// </summary>
        bool ContainsKey(TKey key);

        /// <inheritdoc cref="ContainsKey(TKey)"/>
        /// <summary>
        /// sets <paramref name="value"/> to the found entry's value if such key exists.
        /// </summary>
        bool TryGetValue(TKey key, out TVal value);
    }
}