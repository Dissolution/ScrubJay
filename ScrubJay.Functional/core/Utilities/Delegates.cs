namespace ScrubJay.Functional.Utilities;

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
public delegate bool TryInvoke<T>(out T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    ;