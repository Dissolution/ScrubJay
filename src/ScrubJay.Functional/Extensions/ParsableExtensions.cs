using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class ParsableExtensions
{
#if NET7_0_OR_GREATER
    extension<P>(P)
        where P : IParsable<P>
    {
        public static Result<P, Exception> TryParse([AllowNull, NotNullWhen(true)] string? str, IFormatProvider? provider = null)
        {
            if (P.TryParse(str, provider, out var value))
                return value;
            return ParseException.Create<P>(str);
        }
    }

    extension<P>(P)
        where P : ISpanParsable<P>
    {
        public static Result<P, Exception> TryParse(text text, IFormatProvider? provider = null)
        {
            if (P.TryParse(text, provider, out var value))
                return value;
            return ParseException.Create<P>(text);
        }
    }
#endif
}

public static class SpanFormattableExtensions
{
#if NET6_0_OR_GREATER
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
#endif
}