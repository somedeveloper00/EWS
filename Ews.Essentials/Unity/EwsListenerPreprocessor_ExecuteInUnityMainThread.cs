using System;
using System.Collections.Generic;
using Ews.Core;
using Ews.Core.Interfaces;
using UnityEngine;

namespace Ews.Essentials.Unity
{
    /// <summary>
    /// This is a preprocessor that executes the event in Unity's main thread.
    /// </summary>
    public sealed class EwsPreprocessorExecuteInUnityMainThread : MonoBehaviour,
        IListenerPreprocess, IConnectionEventsPreprocess
    {
        private readonly Queue<Action> _queue = new(16);

        private void Update()
        {
            while (_queue.TryDequeue(out var action))
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        void IListenerPreprocess.ExecuteNewEvent(EwsClient client, byte[] message, IEwsEventListener listener)
        {
            _queue.Enqueue(() => listener.Process(client, message));
        }

        void IConnectionEventsPreprocess.Connected(Action callback) => _queue.Enqueue(callback);
        void IConnectionEventsPreprocess.Disconnected(Action callback) => _queue.Enqueue(callback);
        void IConnectionEventsPreprocess.ReconnectAttempted(Action callback) => _queue.Enqueue(callback);
        void IConnectionEventsPreprocess.OnError(Action callback) => _queue.Enqueue(callback);
    }
}