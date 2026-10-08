#if NET7_0_OR_GREATER
using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Functional;

[PublicAPI]
public static class ParsableExtensions
{
    extension<P>(P)
        where P : IParsable<P>
    {
        public static Result<P> TryParse([AllowNull, NotNullWhen(true)] string? str, IFormatProvider? provider = null)
        {
            if (P.TryParse(str, provider, out var value))
                return value;
            return ParseException.Create<P>(str);
        }
    }

    extension<P>(P)
        where P : ISpanParsable<P>
    {
        public static Result<P> TryParse(text text, IFormatProvider? provider = null)
        {
            if (P.TryParse(text, provider, out var value))
                return value;
            return ParseException.Create<P>(text);
        }
    }
}
#endif