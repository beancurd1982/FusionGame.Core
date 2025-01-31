using System;

namespace FusionGame.Core.Layout
{
    /// <summary>
    /// Defines a contract for handling transitions between different views represented by an enum type.
    /// This interface ensures that any implementing class provides functionality to track the current view 
    /// and transition between views with a specified duration and completion callback.
    /// </summary>
    /// <typeparam name="T">The enum type representing different views in the system.</typeparam>
    public interface ITransition<T> where T : Enum
    {
        /// <summary>
        /// Gets the current active view.
        /// </summary>
        T CurrentView { get; }

        /// <summary>
        /// Performs a transition from one view to another over a specified duration.
        /// </summary>
        /// <param name="fromView">The starting view of the transition.</param>
        /// <param name="toView">The target view to transition into.</param>
        /// <param name="duration">The transition duration in seconds.</param>
        /// <param name="onFinished">A callback function that executes when the transition is complete.</param>
        void DoTransition(T fromView, T toView, float duration, Action onFinished);
    }
}