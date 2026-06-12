namespace ScrubJay.Universal;

/// <summary>
/// An <see cref="Action{T}"/> that references the <paramref name="item"/> so it can be mutated.
/// </summary>
/// <typeparam name="T">
/// The <see cref="Type"/> of <paramref name="item"/> referenced.
/// </typeparam>
/// <param name="item">
/// The <typeparamref name="T"/> item being referenced.
/// </param>
[PublicAPI]
public delegate void RefItem<T>(ref T item)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
;