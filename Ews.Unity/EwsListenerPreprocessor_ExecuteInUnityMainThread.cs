using System;
using System.Collections.Generic;
using Ews.Core;
using Ews.Core.Interfaces;
using UnityEngine;

namespace Ews.Unity
{
    /// <summary>
    /// This is a preprocessor that executes the event in Unity's main thread.
    /// </summary>
    public sealed class EwsListenerPreprocessor_ExecuteInUnityMainThread : MonoBehaviour, IListenerPreprocess
    {
        private readonly Queue<Action> _queue = new(16);

        public void ExecuteNewEvent(EwsClient client, byte[] message, IEwsEventListener listener)
        {
            _queue.Enqueue(() => listener.Process(client, message));
        }

        private void Update()
        {
            while (_queue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
        }
    }
}