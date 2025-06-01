using System;
using System.Collections.Generic;

namespace Assets.FusionGame.Core.Runtime.Data
{
    class EnumValue<T> : IGetValue<T>, ISetValue<T>, IHasSet where T : Enum
    {
        public bool HasSet { get; private set; }
        public T Value { get; private set; }

        public Action<T> OnValueChanged { get; set; }

        public void SetValue(T value)
        {
            var wasSet = HasSet;
            HasSet = true;

            var changed = !EqualityComparer<T>.Default.Equals(Value, value);
            Value = value;

            if (!wasSet || changed)
            {
                OnValueChanged?.Invoke(value);
            }
        }

        public EnumValue(T initialValue = default)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}