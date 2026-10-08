#if NET6_0_OR_GREATER
namespace ScrubJay.Functional;

public static class SpanFormattableExtensions
{
    extension<F>(F formattable)
        where F : ISpanFormattable
    {
        public Option<int> TryFormat(Span<char> destination, text format = default, IFormatProvider? provider = null)
        {
            if (formattable.TryFormat(destination, out int charsWritten, format, provider))
            {
                return Some(charsWritten);
            }
            return default;
        }
    }
}

#endif