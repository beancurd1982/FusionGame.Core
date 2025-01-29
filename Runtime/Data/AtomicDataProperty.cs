using System;

namespace FusionGame.Core.Data
{
    public class AtomicDataProperty<T> : IAtomicDataProperty<T>
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

        public AtomicDataProperty(T initialValue = default)
        {
            value = initialValue;
        }
    }
}
