using System;
using System.Linq;
using System.Reflection;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Layout
{
    public abstract class TransitionBase<T> : ITransition<T> where T : Enum
    {
        public T CurrentView { get; protected set; }
        public NullableEnum<T> TargetView { get; protected set; } = new NullableEnum<T>();
        public bool InTransition { get; protected set; }

        private Action onFinishedCallback;

        public void StartTransition(T fromView, T toView, float duration, Action onFinished)
        {
            if (InTransition)
            {
                throw new InvalidOperationException($"A transition is already in progress from {CurrentView} to {TargetView.Value}. Cannot start a new transition.");
            }

            // Find all methods with the TransitionAttribute attached
            var methodsWithAttribute = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(m =>
                    m.GetCustomAttribute<TransitionAttribute>() != null &&
                    m.GetCustomAttribute<TransitionAttribute>().TypeOfTransitionViewType == typeof(T))
                .ToList();

            foreach (var method in methodsWithAttribute)
            {
                var attribute = method.GetCustomAttribute<TransitionAttribute>();

                // Ensure attribute values match the fromView and toView
                if (attribute.FromViewType is T fromViewTypeEnumValue && fromView.Equals(fromViewTypeEnumValue) &&
                    attribute.ToViewType is T toViewTypeEnumValue && toView.Equals(toViewTypeEnumValue))
                {
                    // Validate method signature (float duration, Action onFinished)
                    var parameters = method.GetParameters();
                    if (parameters.Length == 2 &&
                        parameters[0].ParameterType == typeof(float) &&
                        parameters[1].ParameterType == typeof(Action))
                    {
                        // Set transition state
                        InTransition = true;
                        TargetView.SetValue(toView);
                        onFinishedCallback = onFinished;

                        // Invoke the transition method
                        method.Invoke(this, new object[] { duration, (Action)TransitionCompleted });

                        return;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Method {method.Name} has incorrect parameters. Expected (float duration, Action onFinished)."
                        );
                    }
                }
            }

            throw new InvalidOperationException($"No valid transition method found for {fromView} to {toView}.");
        }

        /// <summary>
        /// Instantly completes the current transition, setting CurrentView to TargetView.
        /// If no transition is active, this method has no effect.
        /// </summary>
        public void FinishTransitionImmediately()
        {
            if (!InTransition) return;

            CurrentView = TargetView.Value;
            TargetView.Clear();
            InTransition = false;

            onFinishedCallback?.Invoke();
            onFinishedCallback = null;
        }

        /// <summary>
        /// Internal method that is called when the transition completes.
        /// </summary>
        private void TransitionCompleted()
        {
            if (!InTransition)
                return;

            Console.WriteLine($"Transition from {CurrentView} to {TargetView.Value} completed.");

            CurrentView = TargetView.Value;
            TargetView.Clear();
            InTransition = false;

            onFinishedCallback?.Invoke();
            onFinishedCallback = null;
        }
    }
}
