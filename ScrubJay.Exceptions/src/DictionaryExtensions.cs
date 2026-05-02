using ScrubJay.Universal.Extensions;

namespace ScrubJay.Exceptions;

[PublicAPI]
public static class DictionaryExtensions
{
    extension(IDictionary dictionary)
    {
        public bool ContainsKey(object key)
        {
            return dictionary.Contains(key);
        }

        public bool TryGetValue(object key, out object? value)
        {
            if (dictionary.Contains(key))
            {
                value = dictionary[key];
                return true;
            }
            value = default;
            return false;
        }

        public object? GetOrAdd(object key, object? valueToAdd)
        {
            if (dictionary.Contains(key))
            {
                return dictionary[key];
            }
            dictionary[key] = valueToAdd;
            return valueToAdd;
        }

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

        public bool TryRemove(object key)
        {
            if (dictionary.Contains(key))
            {
                dictionary.Remove(key);
                return true;
            }
            return false;
        }

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