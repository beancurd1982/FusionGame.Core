using System;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class IntValue : IGetValue<int>, ISetValue<int>, IHasSet
    {
        public bool HasSet { get; private set; }
        public int Value { get; private set; }

        public Action<bool, int, int> OnValueChanged { get; set; }

        public void SetValue(int value)
        {
            var wasSet = HasSet;
            var oldValue = Value;
            HasSet = true;

            var changed = value != Value;
            Value = value;

            if (!wasSet || changed)
            {
                var first = !wasSet;
                var newValue = value; // capture for closure
                ValueChangeDispatcher.Enqueue(() =>
                    OnValueChanged?.Invoke(first, oldValue, newValue));
            }
        }

        public IntValue(int initialValue = 0)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}