using System;
using System.Collections.Generic;
using Ews.Core;
using Ews.Core.Extensions;
using Ews.Core.Interfaces;

namespace Ews.Essentials.Controller
{
    public static class Extensions
    {
        private static readonly Dictionary<IEwsController, IEwsEventListener> s_listeners = new(32);

        /// <summary>
        /// Adds a <see cref="IEwsController{T}"/> to listen to and process upon listen. This is a shortcut to using
        /// <see cref="EwsClientExtensions.AddObjectListener{T}"/>
        /// </summary>
        public static void AddController<T>(this EwsClient client, IEwsController<T> controller, Action<Exception> onError)
            where T : unmanaged
        {
            var listener = client.AddObjectListener<T>(
                eventId: controller.EventId,
                received: data => controller.Process(data, client),
                error: exception => onError?.Invoke(exception));
            s_listeners.Add(controller, listener);
        }

        /// <summary>
        /// Remove <param name="controller"></param> from the <param name="client"></param>, if it had already been added.
        /// Returns whether it was previously added and now removed.
        /// </summary>
        public static bool RemoveController<T1, T2>(this EwsClient client, T1 controller)
            where T1 : unmanaged, IEwsController<T2>
            where T2 : unmanaged
        {
            if (s_listeners.Remove(controller, out var listener))
            {
                client.RemoveListener(controller.EventId, listener);
                return true;
            }
            return false;
        }
    }
}