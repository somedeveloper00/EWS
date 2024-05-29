using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public struct flist4<T> : IFixedList<T> where T : unmanaged
    {
        [field: SerializeField]
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
                CheckCapacity(index + 1);
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

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            CheckCapacity(Count + 1);
            this[Count++] = item;
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            CheckCapacity(index);
            this[index] = item;
            Count = index == Count ? Count + 1 : Count;
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            for (int i = index + 1; i < Count; i++)
            {
                this[i - 1] = this[i];
            }
            Count--;
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
        {
            int index = ((IFixedList<T>)this).IndexOf(item);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i] = default;
            }
            Count = 0;
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> AsSpan()
        {
            unsafe
            {
                fixed (T* ptr = &_0)
                {
                    return new Span<T>(ptr, Count);
                }
            }
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void CheckCapacity(int capacity)
        {
            if (capacity > Capacity)
                throw new IndexOutOfRangeException(capacity.ToString());
        }

        public static implicit operator Span<T>(in flist4<T> flist)
        {
            unsafe
            {
                fixed (T* ptr = &flist._0)
                {
                    return new(ptr, flist.Count);
                }
            }
        }

        public static implicit operator flist4<T>(in Span<T> span)
        {
            var flist = new flist4<T>();
            int count = span.Length > flist.Capacity ? flist.Capacity : span.Length;
            flist.Count = count;
            for (int i = 0; i < count; i++)
                flist[i] = span[i];
            return flist;
        }
    }
}