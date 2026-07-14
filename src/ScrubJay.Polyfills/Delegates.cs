namespace ScrubJay.Polyfills;

[PublicAPI]
public delegate void InAction<T>(in T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate void RefAction<T>(ref T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate void RefReadonlyAction<T>(ref readonly T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate bool InPredicate<T>(in T argument)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate bool RefPredicate<T>(ref T argument)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate bool RefReadOnlyPredicate<T>(ref readonly T argument)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

/// <summary>
/// A <see langword="delegate"/> that represents the <c>bool Try(out var value)</c> pattern.
/// </summary>
[PublicAPI]
public delegate bool TryInvoke<T>(out T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;
    
[PublicAPI]
public delegate bool SpanPredicate<T>(Span<T> span);

[PublicAPI]
public delegate bool ReadOnlySpanPredicate<T>(ReadOnlySpan<T> span);

[PublicAPI]
public delegate bool ScanPredicate<T>(ReadOnlySpan<T> previous, ReadOnlySpan<T> next);