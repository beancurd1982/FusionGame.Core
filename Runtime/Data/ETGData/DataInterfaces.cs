using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
    interface IDestroyable<T>
    {
        void Destroy();
        Action<T> OnBeforeDestroy { get; set; }
    }

    interface IHasSet
    {
        bool HasSet { get; }
    }

    interface ISetValue<in T> //where T : struct
    {
        void SetValue(T value);
    }

    interface IGetValue<T> //where T : struct
    {
        T Value { get; }
        Action<T> OnValueChanged { get; set; }
    }
}