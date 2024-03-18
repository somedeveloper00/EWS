using System;

namespace Ews.Core.Interfaces
{
    /// <summary>
    /// An interface for a stream that can be used to hook into the EWS client to perform operation while 
    /// reading and sending data.
    /// </summary>
    public interface IEwsStreamManip
    {
        /// <summary>
        /// Called when the client reads data from the stream. Don't throw exceptions from this method.
        /// </summary>
        /// <param name="bytes">the bytes that were read.</param>
        /// <param name="count">the number of bytes that were read.</param>
        void OnRead(Span<byte> bytes, int count);

        /// <summary>
        /// Called when the client sends data to the stream. Don't throw exceptions from this method.
        /// </summary>
        /// <param name="bytes">the bytes that were sent.</param>
        void OnSend(byte[] bytes);
    }
}