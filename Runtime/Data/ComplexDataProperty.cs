using System;
using System.Collections.Generic;
using System.Text;

namespace FusionGame.Core.Data
{
    public class ComplexDataProperty<T> : IComplexDataProperty<T> where T : class
    {
        public ComplexDataProperty(T defaultValue)
        {
            value = defaultValue;
        }

        public bool HasSet { get; private set; }

        private T value;

        public T Value => value;

        public event Action<string> OnPropertyChanged;

        public IComplexDataPropertyChangeList CreateChangeList(string changeListName)
        {
            var changeList = new ComplexDataPropertyChangeList<T>(this, changeListName);
            return changeList;
        }

        internal bool ApplyChangeList(IComplexDataPropertyChangeList changeList, out string error)
        {
            var actualChanges = new Dictionary<string, object>();
            var errors = new StringBuilder();
            var hasError = false;

            foreach (var (propertyName, newValue) in changeList.GetChanges())
            {
                var property = typeof(T).GetProperty(propertyName);
                if (property == null)
                {
                    errors.AppendLine($"Property {propertyName} not found in type {typeof(T).Name}");
                    hasError = true;
                    continue;
                }

                if (property.PropertyType != newValue.GetType())
                {
                    errors.AppendLine($"Property {propertyName} type mismatch. Expected {property.PropertyType.Name}, got {newValue.GetType().Name}");
                    hasError = true;
                    continue;
                }

                if (property.GetValue(value).Equals(newValue))
                {
                    continue;
                }

                property.SetValue(value, newValue);
                actualChanges[propertyName] = newValue;
            }

            // raise property changed event after all changes are applied
            if (actualChanges.Count > 0)
            {
                foreach (var propertyName in actualChanges.Keys)
                {
                    OnPropertyChanged?.Invoke(propertyName);
                }
            }

            if (hasError)
            {
                errors.Insert(0, $"Error applying change list: {changeList.Name}\n");
                error = errors.ToString();
                return false;
            }
            else
            {
                error = "";
                return true;
            }
        }
    }
}
