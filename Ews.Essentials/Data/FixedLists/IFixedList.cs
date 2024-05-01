namespace Ews.Essentials.Data
{
    public interface IFixedList<T> where T : unmanaged
    {
        int Count { get; }
        int Capacity { get; }
        T this[int index] { get; set; }
        int IndexOf(T item);
        void Add(T value);
        void Insert(int index, T value);
        void RemoveAt(int index);
        bool Remove(T value);
        void Clear();
    }
}