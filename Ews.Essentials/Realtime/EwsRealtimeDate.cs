using System;

namespace Ews.Essentials.Realtime
{
    /// <summary>
    /// An unmanaged representation of <see cref="DateTime"/> which uses the <see cref="DateTime.Ticks"/> property to
    /// determine the date.
    /// </summary>
    public struct EwsRealtimeDate
    {
        public long ticks;

        public EwsRealtimeDate(DateTime dateTime) => ticks = dateTime.Ticks;

        public DateTime ToDateTime() => new(ticks);
    }
}