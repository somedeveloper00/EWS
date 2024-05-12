using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Ews.Essentials.Data
{
    /// <summary>
    /// rat: ref at. 
    /// ratr: ref at reversed. 
    /// rrat: readonly ref at. 
    /// rratr: readonly ref at reversed. 
    /// </summary>
    public static class FixedListExtensions
    {
        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T2> Select<T1, T2>(this IFixedList<T1> list, Func<T1, T2> predecate)
            where T1 : unmanaged
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return predecate(list[i]);
            }
        }

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [HideInCallstack, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this IFixedList<T> list, Func<T, bool> condition)
            where T : unmanaged
        {
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                if (condition(item))
                    return true;
            }
            return false;
        }
    }
}