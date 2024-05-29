using System;
using System.Collections.Generic;

namespace Ews.Essentials.Data.FixedLists
{
    public static class CollectionExtensions
    {
        public static TList ToFlist<TList, T>(this IEnumerable<T> collection)
            where T : unmanaged
            where TList : struct, IFixedList<T>
        {
            var flist = new TList();
            foreach (var item in collection)
            {
                flist.Add(item);
            }
            return flist;
        }

        public static TList ToFlist<TList, T>(this IList<T> collection)
            where T : unmanaged
            where TList : struct, IFixedList<T>
        {
            var flist = new TList();
            foreach (var item in collection)
            {
                flist.Add(item);
            }
            return flist;
        }

        public static TList ToFlist<TList, T>(this Span<T> collection)
            where T : unmanaged
            where TList : struct, IFixedList<T>
        {
            var flist = new TList();
            foreach (var item in collection)
            {
                flist.Add(item);
            }
            return flist;
        }
    }
}