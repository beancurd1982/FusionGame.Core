using System;

namespace FusionGame.Core.Data
{
    public interface IDataProperty<T>
    {
        bool HasSet { get; }

        T Value { get; }

        event Action<T> OnValueChanged;
    }
}
