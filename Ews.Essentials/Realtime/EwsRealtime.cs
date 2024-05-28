using System;
using Ews.Core;
using Ews.Core.Interfaces;
using Ews.Essentials.Data;
using UnityEngine;

namespace Ews.Essentials.Realtime
{
    /// <summary>
    /// A realtime solution based on <see cref="EwsClient"/>.
    /// </summary>
    public struct EwsRealtime
    {
        public EwsClient client;
        private flist8<Element> elements;

        public EwsRealtime(EwsClient client) : this()
        {
            this.client = client;
        }

        /// <summary>
        /// Tick the system. Calling this function is necessary to continue sending data
        /// </summary>
        public void Tick()
        {
            if (client?.IsConnected() == true)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    elements[i].Process(client, DateTime.UtcNow);
                }
            }
        }

        /// <summary>
        /// Add a new data to be sent in realtime
        /// </summary>
        public unsafe void AddNewData<T>(ref T data, TimeSpan intervals) where T : unmanaged, ITransportingData
        {
            fixed (void* ptr = &data)
            {
                elements.Add(new(intervals, ptr, sizeof(T), data.EventId));
            }
        }

        /// <summary>
        /// Remove a data from being sent realtime
        /// </summary>
        public unsafe bool RemoveData<T>(ref T data) where T : unmanaged, ITransportingData
        {
            fixed (void* ptr = &data)
            {
                for (int i = 0; i < this.elements.Count; i++)
                {
                    if (this.elements[i].ptr == ptr)
                    {
                        this.elements.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        private unsafe struct Element
        {
            public readonly TimeSpan delay;
            public readonly void* ptr;
            public readonly int size;
            public readonly byte eventId;
            public DateTime last;

            public Element(TimeSpan delay, void* ptr, int size, byte eventId) : this()
            {
                this.delay = delay;
                this.ptr = ptr;
                this.size = size;
                this.eventId = eventId;
            }

            public void Process(in EwsClient client, in DateTime dateTime)
            {
                if (dateTime - last > delay)
                {
                    // send
                    var bytes = new Span<byte>(ptr, size);
                    client.Send(eventId, bytes);
                    last = dateTime;
                }
            }
        }
    }
}