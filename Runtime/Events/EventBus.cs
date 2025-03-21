using System;
using System.Collections.Generic;
using UnityEngine;

namespace FusionGame.Core.Events
{
    /// <summary>
    /// Manages event registration, unregistration, and event raising for different event types.
    /// </summary>
    public static class EventBus
    {
        /// <summary>
        /// Represents a binding for an event of type T.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        private class EventBinding<T> : IEventBinding<T> where T : IEvent
        {
            Action<T> onEvent = (T e) => { };

            /// <summary>
            /// Gets or sets the action to be invoked when the event is raised.
            /// </summary>
            Action<T> IEventBinding<T>.OnEvent
            {
                get => onEvent;
                set => onEvent = value;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="EventBinding{T}"/> class.
            /// </summary>
            /// <param name="onEvent">The action to be invoked when the event is raised.</param>
            public EventBinding(Action<T> onEvent)
            {
                this.onEvent = onEvent;
            }

            /// <summary>
            /// Adds an action to the event binding.
            /// </summary>
            /// <param name="action">The action to add.</param>
            public void Add(Action<T> action)
            {
                onEvent += action;
            }

            /// <summary>
            /// Removes an action from the event binding.
            /// </summary>
            /// <param name="action">The action to remove.</param>
            public void Remove(Action<T> action)
            {
                onEvent -= action;
            }
        }

        /// <summary>
        /// Manages event bindings and raising for a specific event type.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        internal static class EventBusGeneric<T> where T : IEvent
        {
            static readonly HashSet<IEventBinding<T>> Bindings = new HashSet<IEventBinding<T>>();
            private static bool raisingEvent = false;

            /// <summary>
            /// Registers an action to be invoked when the event is raised.
            /// </summary>
            /// <param name="onEvent">The action to register.</param>
            /// <returns>The event binding.</returns>
            internal static IEventBinding<T> Register(Action<T> onEvent)
            {
                if (raisingEvent)
                {
                    throw new InvalidOperationException($"Cannot register an {typeof(T)} event binding while an event is being raised.");
                }

                var binding = new EventBinding<T>(onEvent);
                Bindings.Add(binding);
                return binding;
            }

            /// <summary>
            /// Unregisters an event binding.
            /// </summary>
            /// <param name="binding">The event binding to unregister.</param>
            internal static void Unregister(IEventBinding<T> binding)
            {
                if (raisingEvent)
                {
                    throw new InvalidOperationException($"Cannot unregister an {typeof(T)} event binding while an event is being raised.");
                }

                Bindings.Remove(binding);
            }

            /// <summary>
            /// Raises the event, invoking all registered actions.
            /// </summary>
            /// <param name="e">The event to raise.</param>
            internal static void Raise(T e)
            {
                raisingEvent = true;

                foreach (var binding in Bindings)
                {
                    binding.OnEvent.Invoke(e);
                }

                raisingEvent = false;
            }

            /// <summary>
            /// Clears all event bindings. This is called by EventBusUtils.ClearAllBuses().
            /// </summary>
            internal static void Clear()
            {
                Bindings.Clear();
            }
        }

        /// <summary>
        /// Initializes the event bus by bootstrapping event types from the current assembly.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            EventBusUtils.Bootstrap(typeof(EventBus).Assembly);
        }

        /// <summary>
        /// Raises an event of type T.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="e">The event to raise.</param>
        public static void Raise<T>(T e) where T : IEvent
        {
            EventBusGeneric<T>.Raise(e);
        }

        /// <summary>
        /// Registers an action to be invoked when an event of type T is raised.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="onEvent">The action to register.</param>
        /// <returns>The event binding.</returns>
        public static IEventBinding<T> Register<T>(Action<T> onEvent) where T : IEvent
        {
            return EventBusGeneric<T>.Register(onEvent);
        }

        /// <summary>
        /// Unregisters an event binding for an event of type T.
        /// </summary>
        /// <typeparam name="T">The type of event.</typeparam>
        /// <param name="binding">The event binding to unregister.</param>
        public static void Unregister<T>(IEventBinding<T> binding) where T : IEvent
        {
            EventBusGeneric<T>.Unregister(binding);
        }
    }
}