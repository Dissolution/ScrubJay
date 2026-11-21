namespace ScrubJay.Memory;

[PublicAPI]
public delegate bool PrevNextItemsPredicate<T>(ReadOnlySpan<T> previous, ReadOnlySpan<T> next);