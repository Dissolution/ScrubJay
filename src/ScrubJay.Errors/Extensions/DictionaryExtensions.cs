using ScrubJay.Errors.Collections;

namespace ScrubJay.Errors;

internal static class DictionaryExtensions
{
    extension(IDictionary dictionary)
    {
#region Contains
        public bool ContainsKey(object key)
        {
            return dictionary.Contains(key);
        }

        public bool ContainsKey<K>(K key)
            where K : notnull
        {
            return dictionary.Contains(key);
        }

        public bool ContainsKey<K>(K key, IEqualityComparer<K>? keyComparer)
            where K : notnull
        {
            if (keyComparer is null)
                return dictionary.ContainsKey<K>(key);

            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && keyComparer.Equals(k, key))
                    return true;
            }
            return false;
        }

        public bool ContainsValue(object? value)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (object.Equals(entry.Value, value))
                    return true;
            }
            return false;
        }

        public bool ContainsValue<V>(V? value)
        {
            if (value is null)
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    if (entry.Value is null)
                        return true;
                }
            }
            else
            {
                foreach (DictionaryEntry entry in dictionary)
                {
                    if (value.Equals(entry.Value))
                        return true;
                }
            }
            return false;
        }

        public bool ContainsValue<V>(V? value, IEqualityComparer<V>? valueComparer)
        {
            if (valueComparer is null)
                return dictionary.ContainsValue<V>(value);

            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Value is V v && valueComparer.Equals(v!, value!))
                    return true;
            }
            return false;
        }
#endregion

#region TryGetValue
        public bool TryGetValue(object key, out object? value)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (object.Equals(entry.Key, key))
                {
                    value = entry.Value;
                    return true;
                }
            }
            value = default;
            return false;
        }

        public bool TryGetValue<K>(K key, [MaybeNullWhen(false)] out object? value)
            where K : notnull
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && Any.Equals(k, key))
                {
                    value = entry.Value;
                    return true;
                }
            }
            value = default;
            return false;
        }

        public bool TryGetValue<K>(K key, IEqualityComparer<K>? keyComparer, [MaybeNullWhen(false)] out object? value)
            where K : notnull
        {
            if (keyComparer is null)
                return dictionary.TryGetValue<K>(key, out value);

            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && keyComparer.Equals(k, key))
                {
                    value = entry.Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetValue<V>(object key, [MaybeNullWhen(false)] out V value)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (object.Equals(entry.Key, key) && entry.Value is V v)
                {
                    value = v;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetValue<K, V>(K key, [MaybeNullWhen(false)] out V value)
            where K : notnull
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && Any.Equals(k, key) && entry.Value is V v)
                {
                    value = v;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool TryGetValue<K, V>(K key, IEqualityComparer<K>? keyComparer, [MaybeNullWhen(false)] out V value)
            where K : notnull
        {
            if (keyComparer is null)
                return dictionary.TryGetValue<K, V>(key, out value);

            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && keyComparer.Equals(k, key) && entry.Value is V v)
                {
                    value = v;
                    return true;
                }
            }

            value = default;
            return false;
        }
#endregion

#region GetOrAdd
        public object? GetOrAdd(object key, object? valueToAdd)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (object.Equals(entry.Key, key))
                    return entry.Value;
            }
            dictionary.Add(key, valueToAdd);
            return valueToAdd;
        }

        public object? GetOrAdd<K>(K key, object? valueToAdd)
            where K : notnull
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && Any.Equals(k, key))
                    return entry.Value;
            }
            dictionary.Add(key, valueToAdd);
            return valueToAdd;
        }

        public object? GetOrAdd<K>(K key, IEqualityComparer<K>? keyComparer, object? valueToAdd)
            where K : notnull
        {
            if (keyComparer is null)
                return dictionary.GetOrAdd<K>(key, valueToAdd);

            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && Any.Equals(k, key))
                    return entry.Value;
            }
            dictionary.Add(key, valueToAdd);
            return valueToAdd;
        }

        public V GetOrAdd<V>(object key, V valueToAdd)
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (object.Equals(entry.Key, key) && entry.Value is V v)
                    return v;
            }
            dictionary.Add(key, valueToAdd);
            return valueToAdd;
        }

        public V GetOrAdd<K, V>(K key, V valueToAdd)
            where K : notnull
        {
            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && Any.Equals(k, key) && entry.Value is V v)
                    return v;
            }
            dictionary.Add(key, valueToAdd);
            return valueToAdd;
        }

        public V GetOrAdd<K, V>(K key, IEqualityComparer<K>? keyComparer, V valueToAdd)
            where K : notnull
        {
            if (keyComparer is null)
                return dictionary.GetOrAdd<K, V>(key, valueToAdd);

            foreach (DictionaryEntry entry in dictionary)
            {
                if (entry.Key is K k && keyComparer.Equals(k, key) && entry.Value is V v)
                    return v;
            }
            dictionary.Add(key, valueToAdd);
            return valueToAdd;
        }
#endregion


        public bool TryAdd(object key, object? value)
        {
            if (!dictionary.Contains(key))
            {
                dictionary[key] = value;
                return true;
            }
            return false;
        }

        public void Set(object key, object? value)
        {
            dictionary[key] = value;
        }

#region SetMany
        public void SetMany(params ReadOnlySpan<DictionaryEntry> entries)
        {
            foreach (var entry in entries)
            {
                dictionary[entry.Key] = entry.Value;
            }
        }

        public void SetMany<K, V>(params ReadOnlySpan<KeyValuePair<K, V?>> entries)
            where K : notnull
        {
            foreach (var entry in entries)
            {
                dictionary[entry.Key] = entry.Value;
            }
        }

        public void SetMany<K, V>(params ReadOnlySpan<(K Key, V? Value)> entries)
            where K : notnull
        {
            foreach (var entry in entries)
            {
                dictionary[entry.Key] = entry.Value;
            }
        }

        public void SetMany(IEnumerable<DictionaryEntry>? entries)
        {
            if (entries is null) return;

            foreach (var entry in entries)
            {
                dictionary[entry.Key] = entry.Value;
            }
        }

        public void SetMany<K, V>(IEnumerable<KeyValuePair<K, V?>>? entries)
            where K : notnull
        {
            if (entries is null) return;

            foreach (var entry in entries)
            {
                dictionary[entry.Key] = entry.Value;
            }
        }

        public void SetMany<K, V>(IEnumerable<(K Key, V? Value)>? entries)
            where K : notnull
        {
            if (entries is null) return;

            foreach (var entry in entries)
            {
                dictionary[entry.Key] = entry.Value;
            }
        }
        
        public void SetMany(IEnumerable<KeyValue>? keyValues)
        {
            if (keyValues is null) return;

            foreach (var kv in keyValues)
            {
                dictionary[kv.Key] = kv.Value;
            }
        }
#endregion


        

        public void SetOrRemove(object key, object? value)
        {
            if (value is null)
            {
                dictionary.Remove(key);
            }
            else
            {
                dictionary[key] = value;
            }
        }
    }
}