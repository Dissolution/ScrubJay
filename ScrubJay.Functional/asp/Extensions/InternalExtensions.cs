namespace ScrubJay.Functional.Asp.Extensions;

internal static class InternalExtensions
{
    extension(IDictionary dictionary)
    {
        public bool TryGetValue(object key, [MaybeNullWhen(false)] out object? value)
        {
            if (dictionary.Contains(key))
            {
                value = dictionary[key];
                return true;
            }
            value = null;
            return false;
        }
    }
}