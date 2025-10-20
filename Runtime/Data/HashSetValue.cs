using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.FusionGame.Core.Runtime.Data
{
    public class HashSetValue<T> : IGetValue<IReadOnlyCollection<T>>, IHasSet, ICollection<T>
    {
        private readonly HashSet<T> _internalSet = new HashSet<T>();

        public bool HasSet { get; private set; }

        public IReadOnlyCollection<T> Value => _internalSet;

        public Action<IReadOnlyCollection<T>> OnValueChanged { get; set; }

        public int Count => _internalSet.Count;
        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator()
        {
            return _internalSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            var added = _internalSet.Add(item);
            if (added)
            {
                NotifyChanged();
            }
        }

        public void Clear()
        {
            var wasEmpty = _internalSet.Count == 0;
            _internalSet.Clear();
            if (!wasEmpty)
            {
                NotifyChanged();
            }
        }

        public bool Contains(T item)
        {
            return _internalSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalSet.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var removed = _internalSet.Remove(item);
            if (removed)
            {
                NotifyChanged();
            }
            return removed;
        }

        private void NotifyChanged()
        {
            HasSet = true;
            OnValueChanged?.Invoke(_internalSet);
        }
    }
}