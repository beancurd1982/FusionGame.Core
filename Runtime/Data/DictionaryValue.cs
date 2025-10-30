using System;
using System.Collections;
using System.Collections.Generic;
using FusionGame.Core.Utils;

namespace FusionGame.Core.Data
{
    public class DictionaryValue<TKey, TValue> : IGetValue<IReadOnlyDictionary<TKey, TValue>>, IHasSet, IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _internalDictionary = new Dictionary<TKey, TValue>();

        public bool HasSet { get; private set; }

        public IReadOnlyDictionary<TKey, TValue> Value => _internalDictionary;

        public Action<bool, IReadOnlyDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>> OnValueChanged { get; set; }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _internalDictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(TKey key, TValue value)
        {
            var oldValue = new Dictionary<TKey, TValue>(_internalDictionary);
            _internalDictionary.Add(key, value);
            NotifyChanged(oldValue);
        }

        public bool ContainsKey(TKey key) => _internalDictionary.ContainsKey(key);

        public bool Remove(TKey key)
        {
            if (!_internalDictionary.ContainsKey(key)) return false;
            var oldValue = new Dictionary<TKey, TValue>(_internalDictionary);
            var removed = _internalDictionary.Remove(key);
            if (removed) NotifyChanged(oldValue);
            return removed;
        }

        public bool TryGetValue(TKey key, out TValue value) => _internalDictionary.TryGetValue(key, out value);

        public TValue this[TKey key]
        {
            get => _internalDictionary[key];
            set
            {
                var exists = _internalDictionary.TryGetValue(key, out var oldItem);
                if (exists && EqualityComparer<TValue>.Default.Equals(oldItem, value))
                    return;
                var oldValue = new Dictionary<TKey, TValue>(_internalDictionary);
                _internalDictionary[key] = value;
                NotifyChanged(oldValue);
            }
        }

        public ICollection<TKey> Keys => _internalDictionary.Keys;
        public ICollection<TValue> Values => _internalDictionary.Values;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            var oldValue = new Dictionary<TKey, TValue>(_internalDictionary);
            ((IDictionary<TKey, TValue>)_internalDictionary).Add(item);
            NotifyChanged(oldValue);
        }

        public void Clear()
        {
            if (_internalDictionary.Count == 0) return;
            var oldValue = new Dictionary<TKey, TValue>(_internalDictionary);
            _internalDictionary.Clear();
            NotifyChanged(oldValue);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => ((IDictionary<TKey, TValue>)_internalDictionary).Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary<TKey, TValue>)_internalDictionary).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!((IDictionary<TKey, TValue>)_internalDictionary).Contains(item)) return false;
            var oldValue = new Dictionary<TKey, TValue>(_internalDictionary);
            var removed = ((IDictionary<TKey, TValue>)_internalDictionary).Remove(item);
            if (removed) NotifyChanged(oldValue);
            return removed;
        }

        public int Count => _internalDictionary.Count;
        public bool IsReadOnly => false;

        private void NotifyChanged(IReadOnlyDictionary<TKey, TValue> oldValue)
        {
            var isFirstSet = !HasSet;
            HasSet = true;
            var newValue = new Dictionary<TKey, TValue>(_internalDictionary);
            ValueChangeDispatcher.Enqueue(() =>
                OnValueChanged?.Invoke(isFirstSet, oldValue, newValue));
        }
    }
}