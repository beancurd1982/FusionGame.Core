using System;
using System.Collections.Generic;

namespace FusionGame.Core.Data
{
    public class ComplexDataPropertyChangeList<T> : IComplexDataPropertyChangeList where T : class
    {
        private readonly ComplexDataProperty<T> complexDataProperty;
        private readonly HashSet<Tuple<string, object>> changes;

        public string Name { get; }

        internal ComplexDataPropertyChangeList(ComplexDataProperty<T> dataProperty, string name)
        {
            Name = name;
            complexDataProperty = dataProperty;
            changes = new HashSet<Tuple<string, object>>();
        }

        public IComplexDataPropertyChangeList AppendChange(string propertyName, object newValue)
        {
            changes.Add(new Tuple<string, object>(propertyName, newValue));
            return this;
        }

        public bool Submit(out string error)
        {
            return complexDataProperty.ApplyChangeList(this, out error);
        }

        public IEnumerable<Tuple<string, object>> GetChanges()
        {
            return changes;
        }
    }
}
