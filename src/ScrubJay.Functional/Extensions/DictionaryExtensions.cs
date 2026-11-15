namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class DictionaryExtensions
{
    extension<K, V>(IDictionary<K, V> dictionary)
        where K : notnull
    {
        public Option<V> TryGetValue(K key)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;
            return default;
        }
    }
}