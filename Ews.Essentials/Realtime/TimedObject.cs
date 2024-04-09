using System;

namespace Ews.Essentials.Realtime
{
    /// <summary>
    /// A data with time attached to it
    /// </summary>
    [Serializable]
    public struct TimedObject<T> where T : unmanaged
    {
        public EwsRealtimeDate date;
        public T data;

        public TimedObject(EwsRealtimeDate date, T data)
        {
            this.date = date;
            this.data = data;
        }

        public TimedObject(T data) : this(new(DateTime.UtcNow), data) { }
    }
}