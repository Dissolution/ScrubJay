namespace ScrubJay.Memory;

[PublicAPI]
public delegate Option<int> PrevNextItemsStep<T>(ReadOnlySpan<T> previous, ReadOnlySpan<T> next);