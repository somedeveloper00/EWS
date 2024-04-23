using System;

namespace Ews.Essentials.Data
{
    /// <summary>
    /// A <see cref="DateTime"/> unmanaged alternative. Uses Unix Epoch in seconds.
    /// </summary>
    [Serializable]
    public struct EwsDateTime
    {
        /// <summary>
        /// Unix Epoch ticks in seconds
        /// </summary>
        public long ticks;
        
        public override readonly string ToString() => ticks.ToString();

        public EwsDateTime(long ticks) => this.ticks = ticks;

        public EwsDateTime(DateTime dt) => ticks = (long)(dt - DateTime.UnixEpoch).TotalSeconds;

        public DateTime GetDateTime() => DateTime.UnixEpoch.AddSeconds(ticks);
    }
}