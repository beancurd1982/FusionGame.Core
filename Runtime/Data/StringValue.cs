using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
    public class StringValue : IGetValue<string>, ISetValue<string>, IHasSet
    {
        public bool HasSet { get; private set; }
        public string Value { get; private set; }

        public Action<bool, string, string> OnValueChanged { get; set; }

        public void SetValue(string value)
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

        public StringValue(string initialValue = null)
        {
            Value = initialValue;
            HasSet = false;
        }
    }
}