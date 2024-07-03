using System;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public struct fdic16<TKey, TVal> : IFixedDictionary<TKey, TVal>
        where TKey : unmanaged
        where TVal : unmanaged
    {
        private flist16<flist4<Slot>> bucket;

        public ref TVal this[TKey key]
        {
            get
            {
                FindInternal(key, out var i, out var j);
                return ref bucket[i][j].value;
            }
        }

        public readonly int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int count = 0;
                for (int i = 0; i < bucket.Capacity; i++)
                    for (int j = 0; j < bucket[i].Capacity; j++)
                    {
                        count += bucket[i][j].hash == 0 ? 0 : 1;
                    }
                return count;
            }
        }

        public readonly int Capacity => bucket.Capacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Add(TKey key, TVal val)
        {
            bool found = FindInternal(key, out var i, out var j);
            bucket[i][j].value = val;
            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => bucket.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key) => FindInternal(key, out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(TKey key)
        {
            bool found = FindInternal(key, out var i, out var j);
            if (!found)
            {
                bucket[i][j] = default;
            }
            return !found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, ref TVal value)
        {
            bool found = FindInternal(key, out var i, out var j);
            if (found)
                value = ref bucket[i][j].value;
            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindInternal(TKey key, out int i, out int j)
        {
            int fullHash = key.GetHashCode() + 1; // +1 because we don't want it to be 0
            Assert.AreEqual(fullHash, 0, $"hash of type {typeof(TKey).FullName} should not be -1.");
            i = fullHash % bucket.Capacity;
            for (j = 0; j < bucket[i].Capacity; j++)
            {
                ref var bucketEntry = ref bucket[i][j];
                if (bucketEntry.hash == fullHash)
                {
                    return false;
                }
                else if (bucketEntry.hash == 0)
                {
                    return true;
                }
            }
            throw new IndexOutOfRangeException(bucket.Capacity.ToString());
        }

        [Serializable]
        private struct Slot
        {
            /// <summary>
            /// Full hash of the key
            /// </summary>
            public int hash;

            /// <summary>
            /// Value for this slot
            /// </summary>
            public TVal value;
        }
    }

    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public struct fdic32<TKey, TVal> : IFixedDictionary<TKey, TVal>
        where TKey : unmanaged
        where TVal : unmanaged
    {
        private flist32<flist4<Slot>> bucket;

        public ref TVal this[TKey key]
        {
            get
            {
                FindInternal(key, out var i, out var j);
                return ref bucket[i][j].value;
            }
        }

        public readonly int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int count = 0;
                for (int i = 0; i < bucket.Capacity; i++)
                    for (int j = 0; j < bucket[i].Capacity; j++)
                    {
                        count += bucket[i][j].hash == 0 ? 0 : 1;
                    }
                return count;
            }
        }

        public readonly int Capacity => bucket.Capacity;

        public readonly int InnerCapacity => bucket[0].Capacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Add(TKey key, TVal val)
        {
            bool found = FindInternal(key, out var i, out var j);
            bucket[i][j].value = val;
            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => bucket.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key) => FindInternal(key, out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(TKey key)
        {
            bool found = FindInternal(key, out var i, out var j);
            if (!found)
            {
                bucket[i][j] = default;
            }
            return !found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, ref TVal value)
        {
            bool found = FindInternal(key, out var i, out var j);
            if (found)
                value = ref bucket[i][j].value;
            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindInternal(TKey key, out int i, out int j)
        {
            int fullHash = key.GetHashCode() + 1; // +1 because we don't want it to be 0
            Assert.AreEqual(fullHash, 0, $"hash of type {typeof(TKey).FullName} should not be -1.");
            i = fullHash % bucket.Capacity;
            for (j = 0; j < bucket[i].Capacity; j++)
            {
                ref var bucketEntry = ref bucket[i][j];
                if (bucketEntry.hash == fullHash)
                {
                    return false;
                }
                else if (bucketEntry.hash == 0)
                {
                    return true;
                }
            }
            throw new IndexOutOfRangeException(bucket.Capacity.ToString());
        }

        [Serializable]
        private struct Slot
        {
            /// <summary>
            /// Full hash of the key
            /// </summary>
            public int hash;

            /// <summary>
            /// Value for this slot
            /// </summary>
            public TVal value;
        }
    }

    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public struct fdic128<TKey, TVal> : IFixedDictionary<TKey, TVal>
        where TKey : unmanaged
        where TVal : unmanaged
    {
        private flist128<flist4<Slot>> bucket;

        public ref TVal this[TKey key]
        {
            get
            {
                FindInternal(key, out var i, out var j);
                return ref bucket[i][j].value;
            }
        }

        public readonly int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                int count = 0;
                for (int i = 0; i < bucket.Capacity; i++)
                    for (int j = 0; j < bucket[i].Capacity; j++)
                    {
                        count += bucket[i][j].hash == 0 ? 0 : 1;
                    }
                return count;
            }
        }

        public readonly int Capacity => bucket.Capacity;

        public readonly int InnerCapacity => bucket[0].Capacity;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Add(TKey key, TVal val)
        {
            bool found = FindInternal(key, out var i, out var j);
            bucket[i][j].value = val;
            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => bucket.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsKey(TKey key) => FindInternal(key, out _, out _);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(TKey key)
        {
            bool found = FindInternal(key, out var i, out var j);
            if (!found)
            {
                bucket[i][j] = default;
            }
            return !found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(TKey key, ref TVal value)
        {
            bool found = FindInternal(key, out var i, out var j);
            if (found)
                value = ref bucket[i][j].value;
            return found;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool FindInternal(TKey key, out int i, out int j)
        {
            int fullHash = key.GetHashCode() + 1; // +1 because we don't want it to be 0
            Assert.AreEqual(fullHash, 0, $"hash of type {typeof(TKey).FullName} should not be -1.");
            i = fullHash % bucket.Capacity;
            for (j = 0; j < bucket[i].Capacity; j++)
            {
                ref var bucketEntry = ref bucket[i][j];
                if (bucketEntry.hash == fullHash)
                {
                    return false;
                }
                else if (bucketEntry.hash == 0)
                {
                    return true;
                }
            }
            throw new IndexOutOfRangeException(bucket.Capacity.ToString());
        }

        [Serializable]
        private struct Slot
        {
            /// <summary>
            /// Full hash of the key
            /// </summary>
            public int hash;

            /// <summary>
            /// Value for this slot
            /// </summary>
            public TVal value;
        }
    }
}