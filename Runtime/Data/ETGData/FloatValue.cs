using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
    class FloatValue : IGetValueStrong<float>, ISetValue<float>, IHasSet
    {
        public bool HasSet { get; private set; }
        public float Value { get; private set; }

        public Action<bool, float, float> OnValueChanged { get; set; }

        public void SetValue(float value)
        {
            var wasSet = HasSet;
            HasSet = true;

            var oldValue = Value;
            var changed = Math.Abs(value - Value) > float.Epsilon;
            Value = value;

            if (!wasSet || changed)
            {
                OnValueChanged?.Invoke(!wasSet, oldValue, value);
            }
        }

        public FloatValue(float initialValue = 0.0f)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}