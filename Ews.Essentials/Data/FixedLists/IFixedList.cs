using System;

namespace Ews.Essentials.Data
{
    public interface IFixedList<T> where T : unmanaged
    {
        int Count { get; set; }
        int Capacity { get; }
        ref T this[int index] { get; }
        int IndexOf(T item);
        void Add(T value);
        void Insert(int index, T value);
        void RemoveAt(int index);
        bool Remove(T value);
        void Clear();
        Span<T> AsSpan();
    }
}