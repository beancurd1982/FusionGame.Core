using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.FusionGame.Core.Runtime.Data
{
    class ListValue<T> : IGetValue<IReadOnlyList<T>>, IHasSet, IList<T>
    {
        private readonly List<T> _internalList = new List<T>();

        public bool HasSet { get; private set; }

        public IReadOnlyList<T> Value => _internalList;

        public Action<IReadOnlyList<T>> OnValueChanged { get; set; }
        public IEnumerator<T> GetEnumerator()
        {
            return _internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _internalList.Add(item);
            NotifyChanged();
        }

        public void Clear()
        {
            var wasEmpty = _internalList.Count == 0;
            _internalList.Clear();

            if (!wasEmpty)
            {
                NotifyChanged();
            }
        }

        public bool Contains(T item)
        {
            return _internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var removed = _internalList.Remove(item);

            if (removed)
            {
                NotifyChanged();
            }

            return removed;
        }

        public int Count => _internalList.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            return _internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _internalList.Insert(index, item);
            NotifyChanged();
        }

        public void RemoveAt(int index)
        {
            _internalList.RemoveAt(index);
            NotifyChanged();
        }

        public T this[int index]
        {
            get => _internalList[index];
            set
            {
                if (EqualityComparer<T>.Default.Equals(_internalList[index], value)) return;

                _internalList[index] = value;
                NotifyChanged();
            }
        }

        private void NotifyChanged()
        {
            HasSet = true;
            OnValueChanged?.Invoke(_internalList);
        }
    }
}