using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public struct flist4<T> : IFixedList<T> where T : unmanaged
    {
        [field: UnityEngine.SerializeField]
        public int Count { get; set; }
        public T _0;
        public T _1;
        public T _2;
        public T _3;

        public readonly int Capacity => 4;

        public unsafe ref T this[int index]
        {
            get
            {
                BoundsCheck(index);
                fixed (void* ptr = &_0)
                {
                    return ref UnsafeUtility.ArrayElementAsRef<T>(ptr, index);
                }
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Equals(item))
                {
                    return i;
                }
            }
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
            int index = ((IFixedList<T>)this).IndexOf(item);
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