using System;

namespace Ews.Core.Interfaces
{
    /// <summary>
    /// A preprocessor that can decide when the callback is triggered, for example it can change the thread.
    /// </summary>
    public interface IConnectionEventsPreprocess
    {
        void Connected(Action callback);
        void Disconnected(Action callback);
        void ReconnectAttempted(Action callback);
        void OnError(Action callback);
    }
}