namespace ScrubJay.Extensions;

[PublicAPI]
public static class SpanExtensions
{
    extension<T>(Span<T> span)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillFrom(scoped ReadOnlySpan<T> source)
        {
            source.CopyTo(span);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillFrom(scoped Span<T> source)
        {
            source.CopyTo(span);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void FillFrom(T[]? source)
        {
            source.CopyTo(span);
        }
    }
}