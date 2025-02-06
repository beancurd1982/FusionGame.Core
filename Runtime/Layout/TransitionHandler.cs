using System;
using System.Linq;
using System.Reflection;
using FusionGame.Core.Utils;
using UnityEngine;

namespace FusionGame.Core.Layout
{
    public abstract class TransitionHandler<T> : MonoBehaviour, ITransition<T> where T : Enum
    {
        protected bool inTransitionSelf;

        protected internal TransitionRoot<T> transitionRoot;

        public T CurrentView { get; protected set; }    //TODO: maybe this needs to be NullableEnum too

        public NullableEnum<T> TargetView { get; protected set; } = new NullableEnum<T>();
        
        public virtual bool InTransition => inTransitionSelf;

        protected Action onFinishedCallback;

        public virtual void StartTransition(T fromView, T toView, float duration, Action onFinished)
        {
            if (transitionRoot == null)
            {
                throw new InvalidOperationException($"Can't start transition on transition handler {gameObject.name} as it's not registered with the transition root yet!");
            }

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
                        inTransitionSelf = true;
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

            //throw new InvalidOperationException($"No valid transition method found for {fromView} to {toView}.");
            DefaultTransitionMethod(fromView, toView, duration, onFinished);
        }

        /// <summary>
        /// This default transition method will be called if the transition method with correct TransitionAttribute attached is not found.
        /// Implement this method for a fall-back transition behaviour (maybe visually hidden)
        /// </summary>
        /// <param name="fromView"></param>
        /// <param name="toView"></param>
        /// <param name="duration"></param>
        /// <param name="onFinished"></param>
        protected virtual void DefaultTransitionMethod(T fromView, T toView, float duration, Action onFinished)
        {

        }

        /// <summary>
        /// Internal method that is called when the transition completes.
        /// </summary>
        private void TransitionCompleted()
        {
            if (!InTransition)
                return;

            CurrentView = TargetView.Value;
            TargetView.Clear();
            inTransitionSelf = false;

            onFinishedCallback?.Invoke();
            onFinishedCallback = null;
        }

        /// <summary>
        /// Instantly completes the current transition, setting CurrentView to TargetView, and invoking the finish callback.
        /// If no transition is active, this method has no effect.
        /// Override this method for extra behaviour such as stopping tween animation, etc.
        /// </summary>
        public virtual void FinishTransitionImmediately()
        {
            if (transitionRoot == null)
            {
                throw new InvalidOperationException($"Can't finish transition immediately on transition handler {gameObject.name} as it's not registered with the transition root yet!");
            }

            if (!InTransition) return;

            CurrentView = TargetView.Value;
            TargetView.Clear();
            inTransitionSelf = false;

            onFinishedCallback?.Invoke();
            onFinishedCallback = null;
        }

        protected virtual void OnDestroy()
        {
            TargetView.Clear();
            inTransitionSelf = false;

            onFinishedCallback?.Invoke();   // this is important as the transition root may still wait for the transition finish callback.
            onFinishedCallback = null;

            TransitionRoot<T>.UnregisterTransitionHandler(this);
        }

        protected virtual void Awake()
        {
            TransitionRoot<T>.RegisterTransitionHandler(this);
        }
    }
}
