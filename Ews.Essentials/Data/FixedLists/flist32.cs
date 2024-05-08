using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public struct flist32<T> : IFixedList<T> where T : unmanaged
    {
        [field: UnityEngine.SerializeField]
        public int Count { get; set; }
        public T _0;
        public T _1;
        public T _2;
        public T _3;
        public T _4;
        public T _5;
        public T _6;
        public T _7;
        public T _8;
        public T _9;
        public T _10;
        public T _11;
        public T _12;
        public T _13;
        public T _14;
        public T _15;
        public T _16;
        public T _17;
        public T _18;
        public T _19;
        public T _20;
        public T _21;
        public T _22;
        public T _23;
        public T _24;
        public T _25;
        public T _26;
        public T _27;
        public T _28;
        public T _29;
        public T _30;
        public T _31;

        public readonly int Capacity => 4;

        public unsafe ref T this[int index]
        {
            get
            {
                fixed (void* ptr = &_0)
                {
                    return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int IndexOf(T item)
        {
            if (_0.Equals(item)) return 0;
            if (_1.Equals(item)) return 1;
            if (_2.Equals(item)) return 2;
            if (_3.Equals(item)) return 3;
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            CheckCapacity(Count + 1);
            this[Count++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            CheckCapacity(index);
            this[index] = item;
            Count = index == Count ? Count + 1 : Count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            for (int i = index + 1; i < Count; i++)
            {
                this[i - 1] = this[i];
            }
            Count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
            {
                this[i] = default;
            }
            Count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void BoundsCheck(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException(index.ToString());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void CheckCapacity(int capacity)
        {
            if (capacity > Capacity)
                throw new IndexOutOfRangeException(capacity.ToString());
        }
    }
}