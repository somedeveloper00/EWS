using System;
using System.Collections.Generic;
using EWS.Interfaces;
using UnityEngine;

namespace EWS.Unity
{
    /// <summary>
    /// This is a preprocessor that executes the event in Unity's main thread.
    /// </summary>
    public class EwsListenerPreprocessor_ExecuteInUnityMainThread : MonoBehaviour, IListenerPreprocess
    {
        private readonly Queue<Action> _queue = new(16);

        public void ExecuteNewEvent(EwsClient client, byte[] message, IEwsEventListener listener)
        {
            _queue.Enqueue(() => listener.Process(client, message));
            enabled = true;
        }

        private void Update()
        {
            while (_queue.TryDequeue(out var action))
            {
                action?.Invoke();
            }
            enabled = false;
        }
    }
}