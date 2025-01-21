using System;

namespace FusionGame.Core.Data
{
    public class DataProperty<T> : IDataProperty<T>
    {
        private T dataValue;

        public DataProperty()
        {

        }

        public DataProperty(T defaultValue)
        {
            dataValue = defaultValue;
        }

        public bool HasSet { get; private set; }

        public event Action<T> OnValueChanged;

        public T Value
        {
            get => dataValue;
            set
            {
                var changed = !HasSet || !dataValue.Equals(value);
                dataValue = value;
                HasSet = true;

                if (changed)
                {
                    OnValueChanged?.Invoke(dataValue);
                }
            }
        }
    }
}
