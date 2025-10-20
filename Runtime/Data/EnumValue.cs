using System;
using System.Collections.Generic;

namespace Assets.FusionGame.Core.Runtime.Data
{
    public class EnumValue<T> : IGetValueStrong<T>, ISetValue<T>, IHasSet where T : struct, Enum
    {
        public bool HasSet { get; private set; }
        public T Value { get; private set; }

        public Action<bool, T, T> OnValueChanged { get; set; }

        public void SetValue(T value)
        {
            var wasSet = HasSet;
            HasSet = true;

            var oldValue = Value;
            var changed = !EqualityComparer<T>.Default.Equals(Value, value);
            Value = value;

            if (!wasSet || changed)
            {
                OnValueChanged?.Invoke(!wasSet, oldValue, value);
            }
        }

        public EnumValue(T initialValue = default)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}