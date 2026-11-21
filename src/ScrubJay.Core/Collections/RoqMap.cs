//using System.Collections.Frozen;
//
//namespace ScrubJay.Collections;
//
//[PublicAPI]
//public static class RoqMap
//{
//    public static RoqMap<K, V> Create<K, V>(params ReadOnlySpan<KeyValuePair<K, V>> pairs)
//        where K : notnull
//    {
//        if (pairs.IsEmpty)
//            return RoqMap<K, V>.Empty;
//
//        return new(pairs);
//    }
//
//    public static RoqMap<K, V> Create<K, V>(params ReadOnlySpan<(K Key, V Value)> pairs)
//        where K : notnull
//    {
//        if (pairs.IsEmpty)
//            return RoqMap<K, V>.Empty;
//
//        return new(pairs);
//    }
//
//    public static RoqMapBuilder<K, V> Build<K, V>()
//        where K : notnull
//        => RoqMap<K, V>.Build();
//
//    extension<T>(IEnumerable<T>? enumerable)
//    {
//        public RoqMap<K, V> ToRoqMap<K, V>(
//            Func<T, K> getKey,
//            Func<T, V> getValue,
//            IEqualityComparer<K>? keyComparer = default)
//            where K : notnull
//        {
//            if (enumerable is null)
//                return default;
//            return new RoqMap<K, V>(enumerable
//                .Select(value => new KeyValuePair<K, V>(getKey(value), getValue(value))));
//        }
//    }
//}
//
//[PublicAPI]
//[CollectionBuilder(typeof(RoqMap), nameof(RoqMap.Create))]
//[DebuggerDisplay("{DebuggerDisplay,nq}")]
//public readonly struct RoqMap<K, V> :
//    IReadOnlyDictionary<K, V>,
//    IReadOnlyCollection<KeyValuePair<K, V>>,
//    IEnumerable<KeyValuePair<K, V>>,
//    IEqualityOperators<RoqMap<K, V>, RoqMap<K, V>, bool>,
//    IEquatable<RoqMap<K, V>>
//    where K : notnull
//{
//#region Operators
//
//    public static bool operator ==(RoqMap<K, V> left, RoqMap<K, V> right) => left.Equals(right);
//    public static bool operator !=(RoqMap<K, V> left, RoqMap<K, V> right) => !left.Equals(right);
//
//    public static bool operator ==(RoqMap<K, V> left, RoqMap<K, V>? right) => left.Equals(right.GetValueOrDefault());
//    public static bool operator !=(RoqMap<K, V> left, RoqMap<K, V>? right) => !left.Equals(right.GetValueOrDefault());
//
//    public static bool operator ==(RoqMap<K, V>? left, RoqMap<K, V>? right) => left.GetValueOrDefault().Equals(right.GetValueOrDefault());
//    public static bool operator !=(RoqMap<K, V>? left, RoqMap<K, V>? right) => !left.GetValueOrDefault().Equals(right.GetValueOrDefault());
//
//#endregion
//
//    public static readonly RoqMap<K, V> Uninitialized;
//
//    public static readonly RoqMap<K, V> Empty = new(FrozenDictionary<K, V>.Empty);
//
//    public static RoqMap<K, V> Create(params ReadOnlySpan<KeyValuePair<K, V>> values) => new RoqMap<K, V>(values);
//
//    public static RoqMapBuilder<K, V> Build() => new RoqMapBuilder<K, V>();
//
//    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
//    private readonly FrozenDictionary<K, V>? _dictionary;
//
//    V IReadOnlyDictionary<K, V>.this[K key] => this[key];
//
//    public ref readonly V this[K key]
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get
//        {
//            return ref _dictionary![key];
//        }
//    }
//
//    IEnumerable<K> IReadOnlyDictionary<K, V>.Keys => Keys;
//
//    public View<K> Keys
//    {
//        get
//        {
//            var keys = _dictionary!.Keys;
//            return keys.ToView();
//        }
//    }
//
//    IEnumerable<V> IReadOnlyDictionary<K, V>.Values => Values;
//
//    public View<V> Values
//    {
//        get
//        {
//            var values = _dictionary!.Values;
//            return values.ToView();
//        }
//    }
//
//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    public int Length
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _dictionary!.Count;
//    }
//
//    public int Count
//    {
//        get
//        {
//            if (_dictionary is not null)
//                return _dictionary.Count;
//
//            return 0;
//        }
//    }
//
//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    public bool IsDefault
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _dictionary == null;
//    }
//
//    /// <summary>
//    /// Does this <see cref="RoqMap{K,V}"/> have a <see cref="Length"/> of <c>0</c>?
//    /// </summary>
//    /// <exception cref="NullReferenceException">
//    /// Thrown if this <see cref="RoqMap{K,V}"/> is <see cref="Uninitialized"/>.
//    /// </exception>
//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    public bool IsEmpty
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _dictionary!.Count == 0;
//    }
//
//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    public bool IsDefaultOrEmpty
//    {
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        get => _dictionary == null || _dictionary.Count == 0;
//    }
//
//    public RoqMap(FrozenDictionary<K, V>? frozenDictionary)
//    {
//        _dictionary = frozenDictionary;
//    }
//
//    public RoqMap(Dictionary<K, V>? dictionary)
//    {
//        _dictionary = dictionary?.ToFrozenDictionary();
//    }
//
//    public RoqMap(params ReadOnlySpan<KeyValuePair<K, V>> pairs)
//    {
//        _dictionary = FrozenDictionary.Create(pairs);
//    }
//
//    public RoqMap(params ReadOnlySpan<(K Key, V Value)> pairs)
//    {
//        if (pairs.IsEmpty)
//        {
//            _dictionary = [];
//        }
//        else
//        {
//            var dict = new Dictionary<K, V>();
//            foreach (var pair in pairs)
//            {
//                dict[pair.Key] = pair.Value;
//            }
//
//            _dictionary = dict.ToFrozenDictionary();
//        }
//    }
//
//    public RoqMap(IEnumerable<KeyValuePair<K, V>>? pairs)
//    {
//        if (pairs is not null)
//        {
//            _dictionary = pairs.ToFrozenDictionary();
//        }
//    }
//
//    public bool Any()
//    {
//        return _dictionary is not null && _dictionary.Count > 0;
//    }
//
//    public bool ContainsKey(K key)
//    {
//        if (_dictionary is not null)
//        {
//            return _dictionary.ContainsKey(key);
//        }
//
//        return false;
//    }
//
//    public bool TryGetValue(K key, [MaybeNullWhen(false)] out V value)
//    {
//        if (_dictionary is not null)
//        {
//            return _dictionary.TryGetValue(key, out value);
//        }
//
//        value = default;
//        return false;
//    }
//
//    public bool Equals(RoqMap<K, V> roqMap)
//    {
//        return Equals(roqMap._dictionary);
//    }
//
//    public bool Equals(FrozenDictionary<K, V>? frozenDictionary)
//    {
//        var dict = _dictionary;
//        if (ReferenceEquals(dict, frozenDictionary))
//            return true;
//        if (dict is null || frozenDictionary is null)
//            return false;
//
//        int count = dict.Count;
//        if (frozenDictionary.Count != count)
//            return false;
//        if (!ReferenceEquals(dict.Comparer, frozenDictionary.Comparer))
//            return false;
//
//        foreach (var pair in dict)
//        {
//            if (!frozenDictionary.TryGetValue(pair.Key, out var frozenValue))
//                return false;
//            if (!EqualityComparer<V>.Default.Equals(pair.Value, frozenValue))
//                return false;
//        }
//
//        return true;
//    }
//
//    public bool Equals(IReadOnlyDictionary<K, V>? dictionary)
//    {
//        var dict = _dictionary;
//        if (dict is null)
//            return dictionary is null;
//        if (dictionary is null)
//            return false;
//
//        int count = dict.Count;
//        if (dictionary.Count != count)
//            return false;
//
//        foreach (var pair in dict)
//        {
//            if (!dictionary.TryGetValue(pair.Key, out var frozenValue))
//                return false;
//            if (!EqualityComparer<V>.Default.Equals(pair.Value, frozenValue))
//                return false;
//        }
//
//        return true;
//    }
//
//    public override bool Equals([NotNullWhen(true)] object? obj)
//    {
//        return obj switch
//        {
//            RoqMap<K, V> roqMap => Equals(roqMap),
//            FrozenDictionary<K, V> frozenDictionary => Equals(frozenDictionary),
//            Dictionary<K, V> dictionary => Equals(dictionary),
//            IReadOnlyDictionary<K, V> readOnlyDictionary => Equals(readOnlyDictionary),
//            _ => false,
//        };
//    }
//
//    public override int GetHashCode()
//    {
//        var dict = _dictionary;
//        if (dict is null)
//            return 0;
//
//        HashCode hasher = new();
//        foreach (var pair in dict)
//        {
//            hasher.Add(pair.Key, dict.Comparer);
//            hasher.Add(pair.Value);
//        }
//
//        return hasher.ToHashCode();
//    }
//
//    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
//    private string DebuggerDisplay => IsDefault ? "Uninitialized" : $"Count = {Length}";
//
//    public override string ToString()
//    {
//        var dict = _dictionary;
//        if (dict is null)
//            return "null";
//
//        int len = dict.Count;
//        interpolator builder = new(2 + (3 * (len - 1)), len * 2);
//        builder.AppendLiteral("{");
//        if (len > 0)
//        {
//            using var e = dict.GetEnumerator();
//            e.MoveNext();
//            builder.AppendLiteral("[");
//            builder.AppendFormatted(e.Current.Key);
//            builder.AppendLiteral("]=");
//            builder.AppendFormatted(e.Current.Value);
//            while (e.MoveNext())
//            {
//                builder.AppendLiteral(", ");
//                builder.AppendLiteral("[");
//                builder.AppendFormatted(e.Current.Key);
//                builder.AppendLiteral("]=");
//                builder.AppendFormatted(e.Current.Value);
//            }
//        }
//
//        builder.AppendLiteral("}");
//        return builder.ToStringAndClear();
//    }
//
//#region Enumeration
//
//    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//
//    public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
//    {
//        if (_dictionary is null)
//            return Enumerable.Empty<KeyValuePair<K, V>>().GetEnumerator();
//
//        return _dictionary.GetEnumerator();
//    }
//
//#endregion
//}