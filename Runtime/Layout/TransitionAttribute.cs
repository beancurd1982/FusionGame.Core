using System;

namespace FusionGame.Core.Layout
{
    /// <summary>
    /// An attribute used to mark methods as transition methods between different views.
    /// The method marked with this attribute should have exactly two parameters: (float duration, Action onFinished).
    /// It helps in identifying and invoking transition methods dynamically based on the specified transition view type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TransitionAttribute : Attribute
    {
        /// <summary>
        /// The enum type representing the transition view type.
        /// This type must be an enum, ensuring strong type safety for view transitions.
        /// </summary>
        public Type TypeOfTransitionViewType;

        /// <summary>
        /// The source view type from which the transition begins.
        /// This should be an instance of the enum specified by <see cref="TypeOfTransitionViewType"/>.
        /// </summary>
        public object FromViewType;

        /// <summary>
        /// The target view type to which the transition occurs.
        /// This should be an instance of the enum specified by <see cref="TypeOfTransitionViewType"/>.
        /// </summary>
        public object ToViewType;

        /// <summary>
        /// Marks a method as a transition method between views.
        /// The method should have the following signature: (float duration, Action onFinished).
        /// </summary>
        /// <param name="typeOfTransitionViewType">The enum type representing valid transition views.</param>
        /// <param name="fromViewType">The starting view for the transition. Must be a valid value of <paramref name="typeOfTransitionViewType"/>.</param>
        /// <param name="toViewType">The destination view for the transition. Must be a valid value of <paramref name="typeOfTransitionViewType"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="typeOfTransitionViewType"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="typeOfTransitionViewType"/> is not an enum type.</exception>
        public TransitionAttribute(Type typeOfTransitionViewType, object fromViewType, object toViewType)
        {
            if (typeOfTransitionViewType == null)
            {
                throw new ArgumentNullException(nameof(typeOfTransitionViewType));
            }

            if (!typeOfTransitionViewType.IsEnum)
            {
                throw new ArgumentException("typeOfTransitionViewType should be an Enum type", nameof(typeOfTransitionViewType));
            }

            TypeOfTransitionViewType = typeOfTransitionViewType;
            FromViewType = fromViewType;
            ToViewType = toViewType;
        }

        /// <summary>
        /// Retrieves the `FromViewType` value as a strongly typed enum.
        /// </summary>
        /// <typeparam name="T">The expected enum type. Must match <see cref="TypeOfTransitionViewType"/>.</typeparam>
        /// <returns>The `FromViewType` value cast to type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException">Thrown if `FromViewType` cannot be cast to the specified enum type.</exception>
        public T GetFromViewType<T>() where T : Enum
        {
            return (T)FromViewType;
        }
    }

}