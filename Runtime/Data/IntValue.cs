using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
    public class IntValue : IGetValue<int>, ISetValue<int>, IHasSet
    {
        public bool HasSet { get; private set; }
        public int Value { get; private set; }

        public Action<bool, int, int> OnValueChanged { get; set; }

        public void SetValue(int value)
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

        public IntValue(int initialValue = 0)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}