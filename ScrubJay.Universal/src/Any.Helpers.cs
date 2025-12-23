namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(scoped ReadOnlySpan<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(scoped Span<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(scoped ReadOnlySpan<char> text)
    {
        return text.ToString();
    }
}