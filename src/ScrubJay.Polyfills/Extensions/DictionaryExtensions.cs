using System.Collections.Concurrent;
using System.Dynamic;

namespace ScrubJay.Polyfills;

[PublicAPI]
public static class DictionaryExtensions
{
    public static bool IsNotNull([AllowNull, NotNullWhen(true)] this IDictionary? dictionary)
    {
        return dictionary is not null;
    }

    public static bool IsNotEmpty([AllowNull, NotNullWhen(true)] this IDictionary? dictionary)
    {
        return dictionary is not null && dictionary.Count > 0;
    }


    extension<K, V>(Dictionary<K, V> dictionary)
        where K : notnull
    {
        public V GetOrAdd(K key, V valuetoAdd)
        {
            if (dictionary.TryGetValue(key, out var existingValue))
                return existingValue;
            dictionary[key] = valuetoAdd;
            return valuetoAdd;
        }
    }

    extension<K, V>(IDictionary<K, V>? dictionary)
        where K : notnull
    {
        public IEqualityComparer<K>? GetKeyComparer()
        {
            if (dictionary is null)
                return null;
            if (dictionary is Dictionary<K, V> dict)
                return dict.Comparer;
            if (dictionary is ConcurrentDictionary<K, V> concurrentDict)
                return concurrentDict.Comparer;
            return null;
        }


        public IDictionary<K, NV> SelectToDictionary<NV>(
            Func<V, NV> valueSelector,
            IEqualityComparer<K>? keyComparer = null)
        {
            if (dictionary is null)
                return new Dictionary<K, NV>(capacity: 0);

            var dict = new Dictionary<K, NV>(keyComparer ?? dictionary.GetKeyComparer());
            foreach (var pair in dictionary)
            {
                dict[pair.Key] = valueSelector(pair.Value);
            }
            return dict;
        }

        public IDictionary<NK, V> SelectToDictionary<NK>(
            Func<K, NK> keySelector,
            IEqualityComparer<NK>? keyComparer = null)
            where NK : notnull
        {
            if (dictionary is null)
                return new Dictionary<NK, V>(capacity: 0);

            var dict = new Dictionary<NK, V>(keyComparer);
            foreach (var pair in dictionary)
            {
                dict[keySelector(pair.Key)] = pair.Value;
            }
            return dict;
        }

        public IDictionary<NK, NV> SelectToDictionary<NK, NV>(
            Func<K, NK> keySelector,
            Func<V, NV> valueSelector,
            IEqualityComparer<NK>? keyComparer = null)
            where NK : notnull
        {
            if (dictionary is null)
                return new Dictionary<NK, NV>(capacity: 0);

            var dict = new Dictionary<NK, NV>(keyComparer);
            foreach (var pair in dictionary)
            {
                dict[keySelector(pair.Key)] = valueSelector(pair.Value);
            }
            return dict;
        }
    }
}