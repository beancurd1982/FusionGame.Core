using System;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class BooleanValue : IGetValue<bool>, ISetValue<bool>, IHasSet
    {
        public bool HasSet { get; private set; }
        public bool Value { get; private set; }

        public Action<bool, bool, bool> OnValueChanged { get; set; }

        public void SetValue(bool value)
        {
            var wasSet = HasSet;
            var oldValue = Value;
            HasSet = true;

            var changed = value != Value;
            Value = value;

            if (!wasSet || changed)
            {
                var first = !wasSet;
                var newValue = value;
                ValueChangeDispatcher.Enqueue(() =>
                    OnValueChanged?.Invoke(first, oldValue, newValue));
            }
        }

        public BooleanValue(bool initialValue = false)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}