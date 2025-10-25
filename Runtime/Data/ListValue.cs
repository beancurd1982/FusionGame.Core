using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.FusionGame.Core.Runtime.Data
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
            var oldValue = new List<T>(_internalList).AsReadOnly();
            _internalList.Add(item);
            NotifyChanged(oldValue);
        }

        public void Clear()
        {
            if (_internalList.Count == 0) return;
            var oldValue = new List<T>(_internalList).AsReadOnly();
            _internalList.Clear();
            NotifyChanged(oldValue);
        }

        public bool Contains(T item) => _internalList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _internalList.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (!_internalList.Contains(item)) return false;
            var oldValue = new List<T>(_internalList).AsReadOnly();
            var removed = _internalList.Remove(item);
            if (removed) NotifyChanged(oldValue);
            return removed;
        }

        public int Count => _internalList.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item) => _internalList.IndexOf(item);

        public void Insert(int index, T item)
        {
            var oldValue = new List<T>(_internalList).AsReadOnly();
            _internalList.Insert(index, item);
            NotifyChanged(oldValue);
        }

        public void RemoveAt(int index)
        {
            var oldValue = new List<T>(_internalList).AsReadOnly();
            _internalList.RemoveAt(index);
            NotifyChanged(oldValue);
        }

        public T this[int index]
        {
            get => _internalList[index];
            set
            {
                if (EqualityComparer<T>.Default.Equals(_internalList[index], value)) return;
                var oldValue = new List<T>(_internalList).AsReadOnly();
                _internalList[index] = value;
                NotifyChanged(oldValue);
            }
        }

        private void NotifyChanged(IReadOnlyList<T> oldValue)
        {
            var isFirstSet = !HasSet;
            HasSet = true;
            OnValueChanged?.Invoke(isFirstSet, oldValue, _internalList.AsReadOnly());
        }
    }
}