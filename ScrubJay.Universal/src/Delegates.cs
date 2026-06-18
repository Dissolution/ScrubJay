namespace ScrubJay.Universal;

internal delegate string AnyToString<T>(in T? value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate string AnyFormat<T>(in T? value, string? format, IFormatProvider? provider)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate bool AnyTryFormat<T>(in T? value, Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate bool AnyTryParseString<T>(
    [NotNullWhen(true)] string? str,
    IFormatProvider? provider,
    [MaybeNullWhen(false)] out T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate bool AnyTryParseText<T>(
    ReadOnlySpan<char> text,
    IFormatProvider? provider,
    [MaybeNullWhen(false)] out T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate int AnyGetHashCode<T>(in T? value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate bool AnyEqualsObject<T>(in T? value, object? other)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate bool AnyEquals<T>(in T? value, in T? other)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate int AnyCompare<T>(in T? value, in T? other)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate Type AnyGetType<T>(in T? value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

internal delegate void AnyDispose<T>(ref T? value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;



/// <summary>
/// An <see cref="Action{T}"/> that references the <paramref name="item"/> so it can be mutated.
/// </summary>
[PublicAPI]
public delegate void RefItem<T>(ref T item)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;

