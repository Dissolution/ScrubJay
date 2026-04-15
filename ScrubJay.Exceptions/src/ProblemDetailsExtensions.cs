using ScrubJay.Universal;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Exceptions;

[PublicAPI]
public static class ProblemDetailsExtensions
{
    extension(IDictionary dictionary)
    {
        public bool TryGetValue(object key, out object? value)
        {
            if (dictionary.Contains(key))
            {
                value = dictionary[key];
                return true;
            }

            value = null;
            return false;
        }

        public bool TryGetValue<K, V>(K key, out V? value)
            where K :notnull
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
            where K :notnull
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
    }
    
    extension(Exception exception)
    {
        public string Title
        {
            get
            {
                return exception.Data.GetOrAdd<string, string>("Title", Type.Render(in exception))!;
            }
            set
            {
                exception.Data["Title"] = value;
            }
        }
    }
}