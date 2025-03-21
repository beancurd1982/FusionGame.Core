using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FusionGame.Core.Events
{
    /// <summary>
    /// Utility class for managing event bus operations such as bootstrapping and clearing event buses.
    /// </summary>
    public static class EventBusUtils
    {
        /// <summary>
        /// A set of event bus types that have been created.
        /// </summary>
        private static HashSet<Type> EventBusTypes { get; set; } = new HashSet<Type>();

#if UNITY_EDITOR
        /// <summary>
        /// Tracks the current play mode state change in the Unity Editor.
        /// </summary>
        private static PlayModeStateChange PlayModeStateChange { get; set; }

        /// <summary>
        /// Initializes the event bus utilities in the Unity Editor.
        /// </summary>
        [UnityEditor.InitializeOnLoadMethod]
        private static void EditorInitialize()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        /// <summary>
        /// Handles play mode state changes in the Unity Editor.
        /// Clears all event buses when exiting play mode.
        /// </summary>
        /// <param name="state">The new play mode state.</param>
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            PlayModeStateChange = state;
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                ClearAllBuses();
            }
        }
#endif

        /// <summary>
        /// Bootstraps the event buses by scanning the given assembly and creating the event bus types EventBusGeneric<T> for all the concrete classes that implement the IEvent interface.
        /// </summary>
        /// <param name="assembly">The assembly to scan for event types.</param>
        public static void Bootstrap(Assembly assembly)
        {
            var eventTypes = assembly.GetTypes()
                .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var eventType in eventTypes)
            {
                var eventBusType = typeof(EventBus.EventBusGeneric<>).MakeGenericType(eventType);
                EventBusTypes.Add(eventBusType);

                // Force the static constructor to run
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(eventBusType.TypeHandle);
            }
        }

        /// <summary>
        /// Clears all the event buses by calling the static Clear method on each generated event bus type.
        /// </summary>
        public static void ClearAllBuses()
        {
            foreach (var eventBusType in EventBusTypes)
            {
                var clearMethod = eventBusType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
                clearMethod?.Invoke(null, null);
            }
        }
    }
}

