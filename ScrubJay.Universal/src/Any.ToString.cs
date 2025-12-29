namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(T? value)
    {
        if (value is not null)
        {
            return value.ToString()!;
        }

        return string.Empty;
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public static string ToString<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.ToString(value);
    }
#endif

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