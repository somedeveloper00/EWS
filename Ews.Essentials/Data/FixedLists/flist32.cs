using System;
using System.Runtime.CompilerServices;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {_count}")]
    [Serializable]
    public struct flist32<T> : IFixedList<T> where T : unmanaged
    {
        public int _count;
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

        public readonly int Count => _count;
        public readonly int Capacity => 32;

        public T this[int index]
        {
            readonly get => index switch
            {
                0 => _0,
                1 => _1,
                2 => _2,
                3 => _3,
                4 => _4,
                5 => _5,
                6 => _6,
                7 => _7,
                8 => _8,
                9 => _9,
                10 => _10,
                11 => _11,
                12 => _12,
                13 => _13,
                14 => _14,
                15 => _15,
                16 => _16,
                17 => _17,
                18 => _18,
                19 => _19,
                20 => _20,
                21 => _21,
                22 => _22,
                23 => _23,
                24 => _24,
                25 => _25,
                26 => _26,
                27 => _27,
                28 => _28,
                29 => _29,
                30 => _30,
                31 => _31,
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
                    case 4: _4 = value; break;
                    case 5: _5 = value; break;
                    case 6: _6 = value; break;
                    case 7: _7 = value; break;
                    case 8: _8 = value; break;
                    case 9: _9 = value; break;
                    case 10: _10 = value; break;
                    case 11: _11 = value; break;
                    case 12: _12 = value; break;
                    case 13: _13 = value; break;
                    case 14: _14 = value; break;
                    case 15: _15 = value; break;
                    case 16: _16 = value; break;
                    case 17: _17 = value; break;
                    case 18: _18 = value; break;
                    case 19: _19 = value; break;
                    case 20: _20 = value; break;
                    case 21: _21 = value; break;
                    case 22: _22 = value; break;
                    case 23: _23 = value; break;
                    case 24: _24 = value; break;
                    case 25: _25 = value; break;
                    case 26: _26 = value; break;
                    case 27: _27 = value; break;
                    case 28: _28 = value; break;
                    case 29: _29 = value; break;
                    case 30: _30 = value; break;
                    case 31: _31 = value; break;
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
            if (_4.Equals(item)) return 4;
            if (_5.Equals(item)) return 5;
            if (_6.Equals(item)) return 6;
            if (_7.Equals(item)) return 7;
            if (_8.Equals(item)) return 8;
            if (_9.Equals(item)) return 9;
            if (_10.Equals(item)) return 10;
            if (_11.Equals(item)) return 11;
            if (_12.Equals(item)) return 12;
            if (_13.Equals(item)) return 13;
            if (_14.Equals(item)) return 14;
            if (_15.Equals(item)) return 15;
            if (_16.Equals(item)) return 16;
            if (_17.Equals(item)) return 17;
            if (_18.Equals(item)) return 18;
            if (_19.Equals(item)) return 19;
            if (_20.Equals(item)) return 20;
            if (_21.Equals(item)) return 21;
            if (_22.Equals(item)) return 22;
            if (_23.Equals(item)) return 23;
            if (_24.Equals(item)) return 24;
            if (_25.Equals(item)) return 25;
            if (_26.Equals(item)) return 26;
            if (_27.Equals(item)) return 27;
            if (_28.Equals(item)) return 28;
            if (_29.Equals(item)) return 29;
            if (_30.Equals(item)) return 30;
            if (_31.Equals(item)) return 31;
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