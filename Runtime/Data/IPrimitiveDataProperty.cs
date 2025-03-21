using System;

namespace FusionGame.Core.Data
{
    public interface IPrimitiveDataProperty<T> where T : struct
    {
        bool HasSet { get; }

        T Value { get; }

        event Action<T> OnValueChanged;
    }
}
