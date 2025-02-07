using System;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Layout
{
    /// <summary>
    /// Defines a contract for handling transitions between different views represented by an enum type.
    /// This interface ensures that any implementing class provides functionality to track the current view, 
    /// manage transition states, and transition between views over a specified duration with a completion callback.
    /// </summary>
    /// <typeparam name="T">The enum type representing different views in the system.</typeparam>
    internal interface ITransition<T> where T : Enum
    {
        //TODO: see if we need this flag to tell if this component is currently on screen
        //bool IsOnScreen { get; }

        /// <summary>
        /// Gets the current active view in the transition system.
        /// Represents the last fully completed view before a transition.
        /// </summary>
        T CurrentView { get; }

        /// <summary>
        /// Gets the target view being transitioned to.
        /// If null, no transition is currently in progress.
        /// </summary>
        NullableEnum<T> TargetView { get; }

        /// <summary>
        /// Indicates whether a transition is currently in progress.
        /// If true, a transition is happening between <see cref="CurrentView"/> and <see cref="TargetView"/>.
        /// </summary>
        bool InTransition { get; }

        /// <summary>
        /// Starts a transition from one view to another over a specified duration.
        /// </summary>
        /// <param name="fromView">The starting view.</param>
        /// <param name="toView">The target view.</param>
        /// <param name="duration">The transition duration in seconds.</param>
        /// <param name="onFinished">Callback executed when the transition is complete.</param>
        void StartTransition(T fromView, T toView, float duration, Action onFinished);

        /// <summary>
        /// Instantly completes the current transition, setting the <see cref="CurrentView"/> to <see cref="TargetView"/> and trigger the onFinished callback if set previously.
        /// If no transition is active, this method has no effect.
        /// </summary>
        void FinishTransitionImmediately();
    }
}