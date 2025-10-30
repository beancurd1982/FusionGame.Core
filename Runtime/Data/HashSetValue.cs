using System;
using System.Collections;
using System.Collections.Generic;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class HashSetValue<T> : IGetValue<IReadOnlyCollection<T>>, IHasSet, ICollection<T>
    {
        private readonly HashSet<T> _internalSet = new HashSet<T>();

        public bool HasSet { get; private set; }

        public IReadOnlyCollection<T> Value => _internalSet;

        public Action<bool, IReadOnlyCollection<T>, IReadOnlyCollection<T>> OnValueChanged { get; set; }

        public int Count => _internalSet.Count;
        public bool IsReadOnly => false;

        public IEnumerator<T> GetEnumerator() => _internalSet.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            if (_internalSet.Contains(item)) return;
            var oldValue = Snapshot();
            _internalSet.Add(item);
            NotifyChanged(oldValue);
        }

        public void Clear()
        {
            if (_internalSet.Count == 0) return;
            var oldValue = Snapshot();
            _internalSet.Clear();
            NotifyChanged(oldValue);
        }

        public bool Contains(T item) => _internalSet.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _internalSet.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (!_internalSet.Contains(item)) return false;
            var oldValue = Snapshot();
            var removed = _internalSet.Remove(item);
            if (removed) NotifyChanged(oldValue);
            return removed;
        }

        private IReadOnlyCollection<T> Snapshot() => new HashSet<T>(_internalSet);

        private void NotifyChanged(IReadOnlyCollection<T> oldValue)
        {
            var isFirstSet = !HasSet;
            HasSet = true;

            var newValue = Snapshot(); // capture for deferred invoke
            ValueChangeDispatcher.Enqueue(() =>
                OnValueChanged?.Invoke(isFirstSet, oldValue, newValue));
        }
    }
}