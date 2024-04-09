using System;
using System.Threading;
using System.Threading.Tasks;
using Ews.Core;
using Ews.Core.Extensions;

namespace Ews.Essentials.Realtime
{
    public static class EwsRealtimeExtensions
    {
        public static async Task SendObjectOnIntervals<T>(this EwsClient client, byte eventId, TimeSpan intervals,
            Func<T> getObject,
            CancellationToken cancellationToken)
            where T : unmanaged
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(intervals, cancellationToken);
                if (client.IsConnected())
                    client.SendObject(eventId, getObject());
            }
        }

        public static async Task SendTimedObjectOnIntervals<T>(this EwsClient client, byte eventId, TimeSpan intervals,
            Func<T> getObject,
            CancellationToken cancellationToken)
            where T : unmanaged
        {
            var sendingObject = new TimedObject<T>();
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(intervals, cancellationToken);
                if (client.IsConnected())
                {
                    sendingObject.date = new(DateTime.UtcNow);
                    sendingObject.data = getObject();
                    client.SendObject(eventId, sendingObject);
                }
            }
        }
    }
}