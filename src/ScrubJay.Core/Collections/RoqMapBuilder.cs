//namespace ScrubJay.Collections;
//
//[PublicAPI]
//public ref struct RoqMapBuilder<K, V>
//    where K : notnull
//{
//    public static implicit operator RoqMap<K, V>(RoqMapBuilder<K, V> builder) => builder.ToRoqMap();
//
//    private Dictionary<K, V>? _dict;
//
//    public RoqMapBuilder()
//    {
//    }
//
//    public RoqMapBuilder(int capacity)
//    {
//        _dict = new(capacity: capacity);
//    }
//
//    public RoqMapBuilder(IEqualityComparer<K>? keyComparer)
//    {
//        _dict = new(comparer: keyComparer);
//    }
//
//    public RoqMapBuilder(int capacity, IEqualityComparer<K>? keyComparer)
//    {
//        _dict = new(capacity: capacity, comparer: keyComparer);
//    }
//
//    public bool Add(K key, V value)
//    {
//        _dict ??= new();
//        return _dict.TryAdd(key, value);
//    }
//
//    public bool Add(KeyValuePair<K, V> pair)
//    {
//        _dict ??= new();
//        return _dict.TryAdd(pair.Key, pair.Value);
//    }
//
//    public bool Add((K Key, V Value) tuple)
//    {
//        _dict ??= new();
//        return _dict.TryAdd(tuple.Key, tuple.Value);
//    }
//
//    public readonly RoqMap<K, V> ToRoqMap()
//    {
//        return new RoqMap<K, V>(_dict);
//    }
//}