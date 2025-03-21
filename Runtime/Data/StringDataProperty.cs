using Assets.FusionGame.Core.Runtime.Data;
using System;

namespace FusionGame.Core.Data
{
	public class StringDataProperty : IStringDataProperty
	{
        public bool HasSet { get; private set; }
        public string Value { get; private set; }
        public event Action<string> OnValueChanged;

        public void SetValue(string newValue)
        {
            var changed = !HasSet || !Value.Equals(newValue);
            Value = newValue;
            HasSet = true;

            if (changed)
            {
                OnValueChanged?.Invoke(Value);
            }
        }

        public StringDataProperty(string initialValue = "")
        {
            Value = initialValue;
        }
    }
}