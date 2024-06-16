using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public struct flist128<T> : IFixedList<T> where T : unmanaged
    {
        [FormerlySerializedAs("<Count>k__BackingField")]
        public int Count;
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
        public T _32;
        public T _33;
        public T _34;
        public T _35;
        public T _36;
        public T _37;
        public T _38;
        public T _39;
        public T _40;
        public T _41;
        public T _42;
        public T _43;
        public T _44;
        public T _45;
        public T _46;
        public T _47;
        public T _48;
        public T _49;
        public T _50;
        public T _51;
        public T _52;
        public T _53;
        public T _54;
        public T _55;
        public T _56;
        public T _57;
        public T _58;
        public T _59;
        public T _60;
        public T _61;
        public T _62;
        public T _63;
        public T _64;
        public T _65;
        public T _66;
        public T _67;
        public T _68;
        public T _69;
        public T _70;
        public T _71;
        public T _72;
        public T _73;
        public T _74;
        public T _75;
        public T _76;
        public T _77;
        public T _78;
        public T _79;
        public T _80;
        public T _81;
        public T _82;
        public T _83;
        public T _84;
        public T _85;
        public T _86;
        public T _87;
        public T _88;
        public T _89;
        public T _90;
        public T _91;
        public T _92;
        public T _93;
        public T _94;
        public T _95;
        public T _96;
        public T _97;
        public T _98;
        public T _99;
        public T _100;
        public T _101;
        public T _102;
        public T _103;
        public T _104;
        public T _105;
        public T _106;
        public T _107;
        public T _108;
        public T _109;
        public T _110;
        public T _111;
        public T _112;
        public T _113;
        public T _114;
        public T _115;
        public T _116;
        public T _117;
        public T _118;
        public T _119;
        public T _120;
        public T _121;
        public T _122;
        public T _123;
        public T _124;
        public T _125;
        public T _126;
        public T _127;

        public readonly int Capacity => 128;

        int IFixedList<T>.Count { readonly get => Count; set => Count = value; }

        public unsafe ref T this[Index index] => ref index.IsFromEnd ? ref this[Count - 1 - index.Value] : ref this[index.Value];

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
            int index = IndexOf(item);
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

        public static implicit operator Span<T>(in flist128<T> flist)
        {
            unsafe
            {
                fixed (T* ptr = &flist._0)
                {
                    return new(ptr, flist.Count);
                }
            }
        }

        public static implicit operator flist128<T>(in Span<T> span)
        {
            var flist = new flist128<T>();
            int count = span.Length > flist.Capacity ? flist.Capacity : span.Length;
            flist.Count = count;
            for (int i = 0; i < count; i++)
                flist[i] = span[i];
            return flist;
        }
    }
}