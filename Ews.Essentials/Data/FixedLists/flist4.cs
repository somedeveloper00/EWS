using System;
using System.Runtime.CompilerServices;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {_count}")]
    [Serializable]
    public struct flist4<T> : IFixedList<T> where T : unmanaged
    {
        public int _count;
        public T _0;
        public T _1;
        public T _2;
        public T _3;

        public readonly int Count => _count;
        public readonly int Capacity => 4;

        public T this[int index]
        {
            readonly get => index switch
            {
                0 => _0,
                1 => _1,
                2 => _2,
                3 => _3,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };
            set
            {
                switch (index)
                {
                    case 0: _0 = value; break;
                    case 1: _1 = value; break;
                    case 2: _2 = value; break;
                    case 3: _3 = value; break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
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
            CheckCapacity(_count + 1);
            this[_count++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            CheckCapacity(index);
            this[index] = item;
            _count = index == _count ? _count + 1 : _count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            for (int i = index + 1; i < _count; i++)
            {
                this[i - 1] = this[i];
            }
            _count--;
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
            _count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void CheckCapacity(int capacity)
        {
            if (capacity > Capacity)
                throw new IndexOutOfRangeException(capacity.ToString());
        }
    }
}