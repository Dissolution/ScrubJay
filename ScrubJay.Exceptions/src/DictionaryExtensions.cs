using ScrubJay.Universal.Extensions;

namespace ScrubJay.Exceptions;

[PublicAPI]
public static class DictionaryExtensions
{
    extension(IDictionary dictionary)
    {
        public bool ContainsKey<K>(K key)
            where K : notnull
        {
            return dictionary.Contains((object)key);
        }

        public bool TryGetValue<K, V>(K key, out V? value)
            where K : notnull
        {
            object objKey = (object)key;
            if (dictionary.Contains(objKey))
            {
                object? objValue = dictionary[objKey];
                return objValue.Is<V>(out value);
            }
            value = default;
            return false;
        }

        public V? GetOrAdd<K, V>(K key, V? valueToAdd)
            where K : notnull
        {
            object objKey = (object)key;
            if (dictionary.Contains(objKey))
            {
                object? objValue = dictionary[objKey];
                if (objValue is V existingValue)
                    return existingValue;
            }
            dictionary[objKey] = valueToAdd;
            return valueToAdd;
        }

        public bool TryAdd<K, V>(K key, V? value)
            where K : notnull
        {
            object objKey = (object)key;
            if (dictionary.Contains(objKey))
            {
                return false;
            }
            dictionary[objKey] = value;
            return true;
        }

        public void Set<K, V>(K key, V? value)
            where K : notnull
        {
            dictionary[key] = value;
        }

        public bool TryRemove<K>(K key)
            where K : notnull
        {
            object objKey = (object)key;
            if (dictionary.Contains(objKey))
            {
                dictionary.Remove(objKey);
                return true;
            }
            return false;
        }
    }
}