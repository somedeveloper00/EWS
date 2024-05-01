using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ews.Essentials.Data
{
    public static class FixedListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T2> Select<T1, T2>(this IFixedList<T1> list, Func<T1, T2> predecate)
            where T1 : unmanaged
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return predecate(list[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T> Where<T>(this IFixedList<T> list, Func<T, bool> condition)
            where T : unmanaged
        {
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                if (condition(item))
                    yield return item;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T refat<T>(this ref FixedList4<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T refat<T>(this ref FixedList8<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T refat<T>(this ref FixedList16<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                case 8: return ref list._8;
                case 9: return ref list._9;
                case 10: return ref list._10;
                case 11: return ref list._11;
                case 12: return ref list._12;
                case 13: return ref list._13;
                case 14: return ref list._14;
                case 15: return ref list._15;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}