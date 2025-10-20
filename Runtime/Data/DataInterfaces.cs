using System;

namespace Assets.FusionGame.Core.Runtime.Data
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

    public interface ISetValue<in T> //where T : struct
    {
        void SetValue(T value);
    }

    public interface IGetValue<T> //where T : struct
    {
        T Value { get; }
        Action<T> OnValueChanged { get; set; }
    }

    public interface IGetValueStrong<T> where T : struct
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