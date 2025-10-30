using System;

namespace FusionGame.Core.Data
{
    public interface IDestroyable<T>
    {
        void Destroy();
        Action<T> OnBeforeDestroy { get; set; }
    }

    public interface IHasSet
    {
        bool HasSet { get; }
    }

    public interface ISetValue<in T>
    {
        void SetValue(T value);
    }

    public interface IGetValue<T>
    {
        T Value { get; }

        /// <summary>
        /// Event triggered when the value changes.
        /// First parameter indicates if the value was set for the first time (true) or if it was changed (false).
        /// Second parameter is the old value, third parameter is the new value.
        /// </summary>
        Action<bool, T, T> OnValueChanged { get; set; }
    }
}