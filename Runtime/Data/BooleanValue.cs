using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
    public class BooleanValue : IGetValue<bool>, ISetValue<bool>, IHasSet
    {
        public bool HasSet { get; private set; }
        public bool Value { get; private set; }

        public Action<bool, bool, bool> OnValueChanged { get; set; }

        public void SetValue(bool value)
        {
            var wasSet = HasSet;
            HasSet = true;

            var oldValue = Value;
            var changed = value != Value;
            Value = value;

            if (!wasSet || changed)
            {
                OnValueChanged?.Invoke(!wasSet, oldValue, value);
            }
        }

        public BooleanValue(bool initialValue = false)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}