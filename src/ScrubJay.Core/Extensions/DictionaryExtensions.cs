namespace ScrubJay.Extensions;

[PublicAPI]
public static class DictionaryExtensions
{
    extension<K, V>(Dictionary<K, V> dictionary)
        where K : notnull
    {
        public V GetOrAdd(K key, V toAdd)
        {
            if (dictionary.TryGetValue(key, out var existingValue))
                return existingValue;
            dictionary.Add(key, toAdd);
            return toAdd;
        }

        public V GetOrAdd(K key, Func<V> createValue)
        {
            if (dictionary.TryGetValue(key, out var existingValue))
                return existingValue;
            existingValue = createValue();
            dictionary.Add(key, existingValue);
            return existingValue;
        }
    }
}