namespace ScrubJay.Polyfills;

[PublicAPI]
public static class StringExtensions
{
    public static bool IsNotNull([AllowNull, NotNullWhen(true)] this string? str)
    {
        return str is not null;
    }
    
    public static bool IsNotEmpty([AllowNull, NotNullWhen(true)] this string? str)
    {
        return !string.IsNullOrEmpty(str);
    }
}