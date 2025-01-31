using System;
using System.Collections.Generic;

namespace FusionGame.Core.Layout
{
    /// <summary>
    /// Manages and executes transitions between different views represented by an enum type.
    /// This class stores multiple transition items and allows playing a transition from the current view
    /// to a specified target view.
    /// </summary>
    /// <typeparam name="T">The enum type representing different views in the system.</typeparam>
    public class TransitionController<T> where T : Enum
    {
        /// <summary>
        /// A collection of transition items that handle view transitions.
        /// Each item in this list must implement the <see cref="ITransition{T}"/> interface.
        /// </summary>
        private readonly List<ITransition<T>> items = new List<ITransition<T>>();

        /// <summary>
        /// Adds a transition item to the controller.
        /// </summary>
        /// <param name="item">The transition item to add. Must implement <see cref="ITransition{T}"/>.</param>
        public void AddTransitionItem(ITransition<T> item)
        {
            items.Add(item);
        }

        /// <summary>
        /// Plays the transition for a specific transition item from its current view to the specified target view.
        /// </summary>
        /// <param name="itemIndex">The index of the transition item in the list.</param>
        /// <param name="toViewType">The target view type to transition to.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified index is out of range.</exception>
        /// <exception cref="NullReferenceException">Thrown if the transition item at the specified index is null.</exception>
        public void PlayTransition(int itemIndex, T toViewType)
        {
            if (itemIndex < 0 || itemIndex >= items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(itemIndex), "Invalid transition item index.");
            }

            var target = items[itemIndex];
            if (target == null)
            {
                throw new NullReferenceException($"Transition item at index {itemIndex} is null.");
            }

            target.DoTransition(target.CurrentView, toViewType, 1f, null);
        }
    }

}