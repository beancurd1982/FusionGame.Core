using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
    class IntValue : IGetValue<int>, ISetValue<int>, IHasSet
    {
        public bool HasSet { get; private set; }
        public int Value { get; private set; }

        public Action<int> OnValueChanged { get; set; }

        public void SetValue(int value)
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

        public IntValue(int initialValue = 0)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}