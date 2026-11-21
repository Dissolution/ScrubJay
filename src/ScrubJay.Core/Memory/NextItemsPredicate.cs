namespace ScrubJay.Memory;

[PublicAPI]
public delegate bool NextItemsPredicate<T>(ReadOnlySpan<T> span);