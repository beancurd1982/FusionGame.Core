using System;
using System.Linq;
using System.Reflection;

namespace FusionGame.Core.Layout
{
    public abstract class TransitionBase<T> : ITransition<T> where T : Enum
    {
        public T CurrentView { get; protected set; }

        public void DoTransition(T fromView, T toView, float duration, Action onFinished)
        {
            // Find all methods with the TransitionAttribute attached
            var methodsWithAttribute = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(m =>
                    m.GetCustomAttribute<TransitionAttribute>() != null &&
                    m.GetCustomAttribute<TransitionAttribute>().TypeOfTransitionViewType == typeof(T))
                .ToList();

            foreach (var method in methodsWithAttribute)
            {
                var attribute = method.GetCustomAttribute<TransitionAttribute>();

                // Check if the fromView and toView match the attribute values
                if (attribute.FromViewType is T fromViewTypeEnumValue && fromView.Equals(fromViewTypeEnumValue) &&
                    attribute.ToViewType is T toViewTypeEnumValue && toView.Equals(toViewTypeEnumValue))
                {
                    // Check if the method has exactly two parameters
                    var parameters = method.GetParameters();
                    if (parameters.Length == 2 &&
                        parameters[0].ParameterType == typeof(float) &&
                        parameters[1].ParameterType == typeof(Action))
                    {
                        // Invoke the method if all checks pass
                        method.Invoke(this, new object[] { duration, onFinished });
                        return;
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Method {method.Name} has incorrect parameters. Expected (float, Action)."
                        );
                    }
                }
            }
        }
    }
}