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
public delegate void SpanAction<T>(Span<T> span);

[PublicAPI]
public delegate void ReadOnlySpanAction<T>(ReadOnlySpan<T> span);


[PublicAPI]
public delegate R InFunc<T, out R>(in T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate R RefFunc<T, out R>(ref T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate R RefReadonlyFunc<T, out R>(ref readonly T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;

[PublicAPI]
public delegate R SpanFunc<T, out R>(Span<T> span);

[PublicAPI]
public delegate R ReadOnlySpanFunc<T, out R>(ReadOnlySpan<T> span);



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