using System;
using System.Collections.Generic;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class EnumValue<T> : IGetValue<T>, ISetValue<T>, IHasSet where T : struct, Enum
    {
        public bool HasSet { get; private set; }
        public T Value { get; private set; }

        public Action<bool, T, T> OnValueChanged { get; set; }

        public void SetValue(T value)
        {
            var wasSet = HasSet;
            var oldValue = Value;

            HasSet = true;

            var changed = !EqualityComparer<T>.Default.Equals(Value, value);
            Value = value;

            if (!wasSet || changed)
            {
                var first = !wasSet;
                var newValue = value; // capture
                ValueChangeDispatcher.Enqueue(() =>
                    OnValueChanged?.Invoke(first, oldValue, newValue));
            }
        }

        public EnumValue(T initialValue = default)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}