using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.FusionGame.Core.Runtime.Data
{
    class DictionaryValue<TKey, TValue> : IGetValue<IReadOnlyDictionary<TKey, TValue>>, IHasSet, IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _internalDictionary = new Dictionary<TKey, TValue>();

        public bool HasSet { get; private set; }

        public IReadOnlyDictionary<TKey, TValue> Value => _internalDictionary;

        public Action<IReadOnlyDictionary<TKey, TValue>> OnValueChanged { get; set; }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            _internalDictionary.Add(key, value);
            NotifyChanged();
        }

        public bool ContainsKey(TKey key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            var removed = _internalDictionary.Remove(key);
            if (removed)
            {
                NotifyChanged();
            }
            return removed;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _internalDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => _internalDictionary[key];
            set
            {
                var exists = _internalDictionary.TryGetValue(key, out var oldValue);
                if (exists && EqualityComparer<TValue>.Default.Equals(oldValue, value))
                    return;

                _internalDictionary[key] = value;
                NotifyChanged();
            }
        }

        public ICollection<TKey> Keys => _internalDictionary.Keys;
        public ICollection<TValue> Values => _internalDictionary.Values;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)_internalDictionary).Add(item);
            NotifyChanged();
        }

        public void Clear()
        {
            var wasEmpty = _internalDictionary.Count == 0;
            _internalDictionary.Clear();
            if (!wasEmpty)
            {
                NotifyChanged();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)_internalDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)_internalDictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var removed = ((IDictionary<TKey, TValue>)_internalDictionary).Remove(item);
            if (removed)
            {
                NotifyChanged();
            }
            return removed;
        }

        public int Count => _internalDictionary.Count;
        public bool IsReadOnly => false;

        private void NotifyChanged()
        {
            HasSet = true;
            OnValueChanged?.Invoke(_internalDictionary);
        }
    }
}