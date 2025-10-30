using System;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class FloatValue : IGetValue<float>, ISetValue<float>, IHasSet
    {
        public bool HasSet { get; private set; }
        public float Value { get; private set; }

        public Action<bool, float, float> OnValueChanged { get; set; }

        public void SetValue(float value)
        {
            var wasSet = HasSet;
            var oldValue = Value;

            // mark as set before computing changed so first-time fires
            HasSet = true;

            var changed = Math.Abs(value - Value) > float.Epsilon;
            Value = value;

            if (!wasSet || changed)
            {
                var first = !wasSet;
                var newValue = value; // capture
                ValueChangeDispatcher.Enqueue(() =>
                    OnValueChanged?.Invoke(first, oldValue, newValue));
            }
        }

        public FloatValue(float initialValue = 0.0f)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}