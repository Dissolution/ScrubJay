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
}