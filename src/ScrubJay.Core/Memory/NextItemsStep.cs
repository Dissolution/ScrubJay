namespace ScrubJay.Memory;

[PublicAPI]
public delegate Option<int> NextItemsStep<T>(ReadOnlySpan<T> span);