using System;
using System.Threading;
using Ews.Core;
using Ews.Core.Extensions;
using Ews.Core.Interfaces;

namespace Ews.Essentials.Realtime
{
    public static class EwsRealtimeExtensions
    {
        public unsafe static void SendTimedObjectOnIntervals<T>(this EwsClient client, ref T obj, TimeSpan intervals,
            CancellationToken cancellationToken)
            where T : unmanaged, ITransportingData
        {
            fixed (T* ptr = &obj)
            {
                CreateContinousSendThread(client, obj.EventId, intervals, ptr, cancellationToken);
            }
        }

        public unsafe static void SendTimedObjectOnIntervals<T>(this EwsClient client, byte eventId, TimeSpan intervals,
            in T obj,
            CancellationToken cancellationToken)
            where T : unmanaged
        {
            fixed (T* ptr = &obj)
            {
                CreateContinousSendThread(client, eventId, intervals, ptr, cancellationToken);
            }
        }

        private static unsafe Thread CreateContinousSendThread<T>(EwsClient client, byte eventId, TimeSpan intervals, T* ptr, CancellationToken cancellationToken)
            where T : unmanaged
        {
            return new Thread(() =>
            {
                try
                {
                    var sendingObject = new TimedObject<T>();
                    while (true)
                    {
                        Thread.Sleep(intervals);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        if (client.IsConnected())
                        {
                            sendingObject.date = new(DateTime.UtcNow);
                            sendingObject.data = *ptr;
                            client.SendObject(eventId, sendingObject);
                        }
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }, sizeof(TimedObject<T>) + sizeof(DateTime) + sizeof(bool) * 2);
        }
    }
}