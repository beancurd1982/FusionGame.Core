using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
    class BooleanValue : IGetValue<bool>, ISetValue<bool>, IHasSet
    {
        public bool HasSet { get; private set; }
        public bool Value { get; private set; }

        public Action<bool> OnValueChanged { get; set; }

        public void SetValue(bool value)
        {
            var wasSet = HasSet;
            HasSet = true;

            var changed = value != Value;
            Value = value;

            if (!wasSet || changed)
            {
                OnValueChanged?.Invoke(value);
            }
        }

        public BooleanValue(bool initialValue = false)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}