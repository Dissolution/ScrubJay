namespace ScrubJay.Universal;

internal delegate string AnyToString<T>(in T value)
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

/// <summary>
/// A <see langword="delegate"/> that represents the <c>bool Try(out var value)</c> pattern.
/// </summary>
public delegate bool TryInvoke<T>(out T value)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;