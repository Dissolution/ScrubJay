namespace ScrubJay.Testing.Naughty;

public delegate void NaughtyOverlapConsumer<T>(ReadOnlySpan<T> source, Span<T> destination);