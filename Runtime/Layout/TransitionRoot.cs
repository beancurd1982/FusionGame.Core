using System;
using System.Collections.Generic;
using System.Linq;

namespace FusionGame.Core.Layout
{
    public abstract class TransitionRoot<T> : TransitionHandler<T> where T : Enum
    {
        protected internal readonly HashSet<TransitionHandler<T>> RegisteredTransitionHandlers = new HashSet<TransitionHandler<T>>();

        public static void RegisterTransitionHandler(TransitionHandler<T> handler)
        {
            var transitionRoot = handler.GetComponentInParent<TransitionRoot<T>>();
            if (transitionRoot == null)
            {
                throw new InvalidOperationException($"Failed to find the transition root for transition handler {handler.gameObject.name}");
            }

            transitionRoot.Register(handler);
        }

        public static void UnregisterTransitionHandler(TransitionHandler<T> handler)
        {
            if (handler.transitionRoot == null) return;

            handler.transitionRoot.Unregister(handler);
        }

        private void Register(TransitionHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException();

            if (handler == this) return;

            if (handler.transitionRoot != null) return;

            RegisteredTransitionHandlers.Add(handler);
            handler.transitionRoot = this;
        }

        private void Unregister(TransitionHandler<T> handler)
        {
            if (handler == null) throw new ArgumentNullException();

            if (handler.InTransition) throw new InvalidOperationException($"Failed to unregister the transition handler {handler.gameObject.name} as it's still in transition!");

            if (handler == this) return;

            if (RegisteredTransitionHandlers.Contains(handler))
            {
                RegisteredTransitionHandlers.Remove(handler);
                handler.transitionRoot = null;
            }
        }

        public override bool InTransition
        {
            get
            {
                return inTransitionSelf || RegisteredTransitionHandlers.Any(h => h.InTransition);
            }
        }

        public override void StartTransition(T fromView, T toView, float duration, Action onFinished)
        {
            if (InTransition)
            {
                throw new InvalidOperationException($"A transition is already in progress on this root or the children handlers. Cannot start a new transition.");
            }

            var totalTransitionNumber = RegisteredTransitionHandlers.Count + 1;

            base.StartTransition(fromView, toView, duration, () =>
            {
                totalTransitionNumber--;
                if (totalTransitionNumber == 0)
                {
                    onFinished?.Invoke();
                }
            });

            foreach (var handler in RegisteredTransitionHandlers)
            {
                handler.StartTransition(fromView, toView, duration, () =>
                {
                    totalTransitionNumber--;
                    if (totalTransitionNumber == 0)
                    {
                        onFinished?.Invoke();
                    }
                });
            }
        }

        public override void FinishTransitionImmediately()
        {
            base.FinishTransitionImmediately();

            foreach (var handler in RegisteredTransitionHandlers)
            {
                handler.FinishTransitionImmediately();
            }
        }

        protected override void OnDestroy()
        {
            while(RegisteredTransitionHandlers.Count > 0)
            {
                var handler = RegisteredTransitionHandlers.First();
                Unregister(handler);
            }

            base.OnDestroy();
        }
    }
}