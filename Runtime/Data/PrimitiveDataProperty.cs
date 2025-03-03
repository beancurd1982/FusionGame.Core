using System;

namespace FusionGame.Core.Data
{
    public class PrimitiveDataProperty<T> : IPrimitiveDataProperty<T> where T : struct
    {
        public bool HasSet { get; private set; }

        private T value;

        public T Value => value;

        public event Action<T> OnValueChanged;

        public void SetValue(T newValue)
        {
            var changed = !HasSet || !value.Equals(newValue);
            value = newValue;
            HasSet = true;

            if (changed)
            {
                OnValueChanged?.Invoke(value);
            }
        }

        public PrimitiveDataProperty(T initialValue = default)
        {
            value = initialValue;
        }
    }
}
