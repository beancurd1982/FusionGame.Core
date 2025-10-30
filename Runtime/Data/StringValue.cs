using System;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class StringValue : IGetValue<string>, ISetValue<string>, IHasSet
    {
        public bool HasSet { get; private set; }
        public string Value { get; private set; }

        public Action<bool, string, string> OnValueChanged { get; set; }

        public void SetValue(string value)
        {
            var wasSet = HasSet;
            var oldValue = Value;

            HasSet = true;

            var changed = value != Value;
            Value = value;

            if (!wasSet || changed)
            {
                var first = !wasSet;
                var newValue = value; // capture
                ValueChangeDispatcher.Enqueue(() =>
                    OnValueChanged?.Invoke(first, oldValue, newValue));
            }
        }

        public StringValue(string initialValue = null)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}