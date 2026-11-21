namespace ScrubJay.Extensions;

/// <summary>
/// Extensions on <see cref="Span{T}"/>.
/// </summary>
[PublicAPI]
public static class SpanExtensions
{
    extension<T>(Span<T> span)
    {
        public bool TryGetFirst([MaybeNullWhen(false)] out T last)
        {
            if (!span.IsEmpty)
            {
                last = span[0];
                return true;
            }

            last = default;
            return false;
        }

        public Option<T> TryGetFirst()
        {
            if (!span.IsEmpty)
            {
                return span[0];
            }

            return default;
        }
        
        public bool TryGetLast([MaybeNullWhen(false)] out T last)
        {
            if (!span.IsEmpty)
            {
                last = span[^1];
                return true;
            }

            last = default;
            return false;
        }

        public Option<T> TryGetLast()
        {
            if (!span.IsEmpty)
            {
                return span[^1];
            }

            return default;
        }
    }
}

/// <summary>
/// Extensions on <see cref="ReadOnlySpan{T}"/>.
/// </summary>
[PublicAPI]
public static class ReadOnlySpanExtensions
{
}