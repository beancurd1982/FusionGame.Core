using System;
using System.Collections.Generic;
using UnityEngine;

namespace FusionGame.Core.Utils
{
    /// <summary>
    /// Main-thread end-of-frame event dispatcher.
    /// Automatically created before the first scene loads.
    /// </summary>
    internal sealed class ValueChangeDispatcher : MonoBehaviour
    {
        private static ValueChangeDispatcher instance;
        private static readonly Queue<Action> Queue = new Queue<Action>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsureExists()
        {
            if (instance != null) return;

            var go = new GameObject("[ValueChangeDispatcher]");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<ValueChangeDispatcher>();
        }

        /// <summary>
        /// Queue an action to be invoked in LateUpdate on the main thread.
        /// </summary>
        public static void Enqueue(Action action)
        {
            if (action == null) return;
            Queue.Enqueue(action);
        }

        private void LateUpdate()
        {
            // drain the queue to avoid reentrancy
            while (Queue.Count > 0)
            {
                var a = Queue.Dequeue();
                try { a(); }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}