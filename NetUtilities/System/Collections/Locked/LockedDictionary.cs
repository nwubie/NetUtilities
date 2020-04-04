﻿using NetUtilities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Locked
{
    /// <summary>
    /// Provides a thread-safe implementation of <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    public sealed class LockedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICloneable<LockedDictionary<TKey, TValue>>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly object _lock = new object();

        /// <inheritdoc/>
        public TValue this[TKey key] 
        {
            [return: MaybeNull]
            get
            {
                lock (_lock)
                    return _dictionary[key];
            }
            set
            {
                lock (_lock)
                    _dictionary[key] = value;
            }
        }

        /// <inheritdoc/>
        public ICollection<TKey> Keys
        {
            get
            {
                lock (_lock)
                    return _dictionary.Keys.ToHashSet();
            }
        }

        /// <inheritdoc/>
        public ICollection<TValue> Values
        {
            get
            {
                lock (_lock)
                    return _dictionary.Values.ToHashSet();
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                lock (_lock)
                    return _dictionary.Count;
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
            => false;

        /// <summary>
        /// Creates an empty <see cref="LockedDictionary{TKey, TValue}"/>
        /// </summary>
        public LockedDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Creates a <see cref="LockedDictionary{TKey, TValue}"/> from the provided <paramref name="dictionary"/>. 
        /// By default this keeps the reference of the provided dictionary.
        /// </summary>
        /// <param name="dictionary">The source dictionary.</param>
        public LockedDictionary(Dictionary<TKey, TValue> dictionary) : this(dictionary, true)
        {
        }

        /// <summary>
        /// Creates a <see cref="LockedDictionary{TKey, TValue}"/> from the provided <paramref name="dictionary"/>. 
        /// </summary>
        /// <param name="dictionary">The source dictionary.</param>
        /// <param name="keepReference">Indicates if the reference of the source dictionary should be kept.</param>
        public LockedDictionary(Dictionary<TKey, TValue> dictionary, bool keepReference)
        {
            Ensure.NotNull(dictionary);

            _dictionary = keepReference ? dictionary : dictionary.ToDictionary();
        }

        /// <summary>
        /// Creates a <see cref="LockedDictionary{TKey, TValue}"/> from the provided <paramref name="source"/>. 
        /// </summary>
        /// <param name="source">The source.</param>
        public LockedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source) 
        {
            _dictionary = Ensure.NotNull(source).ToDictionary();
        }

        /// <summary>
        /// Creates a <see cref="LockedDictionary{TKey, TValue}"/> from the provided <paramref name="source"/>. 
        /// </summary>
        /// <param name="source">The source.</param>
        public LockedDictionary(IEnumerable<(TKey, TValue)> source)
        {
            _dictionary = Ensure.NotNull(source).ToDictionary();
        }

        /// <inheritdoc/>
        public void Add(TKey key, TValue value)
        {
            lock (_lock)
                _dictionary.Add(key, value);
        }

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> pair)
        {
            lock (_lock)
                _dictionary.Add(pair.Key, pair.Value);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            lock (_lock)
                _dictionary.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_lock)
                return ((IDictionary<TKey, TValue>)_dictionary).Contains(item);
        }

        /// <inheritdoc/>
        public bool ContainsKey(TKey key)
        {
            lock (_lock)
                return _dictionary.ContainsKey(key);
        }

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_lock)
                ((IDictionary<TKey, TValue>)_dictionary).CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public Enumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (_lock)
                return Clone()._dictionary.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            lock (_lock)
                return _dictionary.Remove(key);
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_lock)
                return ((IDictionary<TKey, TValue>)_dictionary).Remove(item);
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            lock (_lock)
                return _dictionary.TryGetValue(key, out value);
        }

        /// <inheritdoc/>
        public LockedDictionary<TKey, TValue> Clone()
        {
            lock (_lock)
                return new LockedDictionary<TKey, TValue>(_dictionary, false);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc/>
        public readonly struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly Dictionary<TKey, TValue>.Enumerator _enumerator;

            /// <inheritdoc/>
            public KeyValuePair<TKey, TValue> Current 
                => _enumerator.Current;

            /// <summary>
            /// Creates an enumerator to though the <see cref="LockedDictionary{TKey, TValue}"/>.
            /// </summary>
            /// <param name="dictionary">The source dictionary.</param>
            public Enumerator(LockedDictionary<TKey, TValue> dictionary)
            {
                _enumerator = dictionary._dictionary.GetEnumerator();
            }

            /// <inheritdoc/>
            public void Dispose()
                => _enumerator.Dispose();

            /// <inheritdoc/>
            public bool MoveNext()
                => _enumerator.MoveNext();

            object? IEnumerator.Current
                => _enumerator.Current;

            void IEnumerator.Reset()
            {
            }
        }
    }
}