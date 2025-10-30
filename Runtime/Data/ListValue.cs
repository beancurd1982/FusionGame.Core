using System;
using System.Collections;
using System.Collections.Generic;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class ListValue<T> : IGetValue<IReadOnlyList<T>>, IHasSet, IList<T>
    {
        private readonly List<T> _internalList = new List<T>();

        public bool HasSet { get; private set; }
        public IReadOnlyList<T> Value => _internalList;

        public Action<bool, IReadOnlyList<T>, IReadOnlyList<T>> OnValueChanged { get; set; }

        public IEnumerator<T> GetEnumerator() => _internalList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            var oldValue = Snapshot();
            _internalList.Add(item);
            NotifyChanged(oldValue);
        }

        public void Clear()
        {
            if (_internalList.Count == 0) return;
            var oldValue = Snapshot();
            _internalList.Clear();
            NotifyChanged(oldValue);
        }

        public bool Contains(T item) => _internalList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _internalList.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (!_internalList.Contains(item)) return false;
            var oldValue = Snapshot();
            var removed = _internalList.Remove(item);
            if (removed) NotifyChanged(oldValue);
            return removed;
        }

        public int Count => _internalList.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item) => _internalList.IndexOf(item);

        public void Insert(int index, T item)
        {
            var oldValue = Snapshot();
            _internalList.Insert(index, item);
            NotifyChanged(oldValue);
        }

        public void RemoveAt(int index)
        {
            var oldValue = Snapshot();
            _internalList.RemoveAt(index);
            NotifyChanged(oldValue);
        }

        public T this[int index]
        {
            get => _internalList[index];
            set
            {
                if (EqualityComparer<T>.Default.Equals(_internalList[index], value)) return;
                var oldValue = Snapshot();
                _internalList[index] = value;
                NotifyChanged(oldValue);
            }
        }

        private IReadOnlyList<T> Snapshot() => new List<T>(_internalList).AsReadOnly();

        private void NotifyChanged(IReadOnlyList<T> oldValue)
        {
            var isFirstSet = !HasSet;
            HasSet = true;

            // capture a read-only view of current list for "newValue" at dispatch time
            var newValue = _internalList.AsReadOnly();
            ValueChangeDispatcher.Enqueue(() =>
                OnValueChanged?.Invoke(isFirstSet, oldValue, newValue));
        }
    }
}