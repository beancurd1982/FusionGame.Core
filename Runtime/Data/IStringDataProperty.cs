using System;

namespace Assets.FusionGame.Core.Runtime.Data
{
	public interface IStringDataProperty
	{
        bool HasSet { get; }
        string Value { get; }
        event Action<string> OnValueChanged;
    }
}