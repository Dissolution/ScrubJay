#if NET7_0_OR_GREATER
using System.Globalization;
using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class NumberExtensions
{
    extension<N>(N)
        where N : INumberBase<N>
    {
        public static Result<N> TryParse(
            [AllowNull, NotNullWhen(true)] string? str,
            NumberStyles numberStyles = default,
            IFormatProvider? provider = null)
        {
            if (N.TryParse(str, numberStyles, provider, out var value))
                return value;

            return ParseException.Create<N>(str, new()
            {
                ("NumberStyles", numberStyles),
                ("IFormatProvider", provider),
            });
        }
        
        public static Result<N> TryParse(
            text text,
            NumberStyles numberStyles = default,
            IFormatProvider? provider = null)
        {
            if (N.TryParse(text, numberStyles, provider, out var value))
                return value;

            return ParseException.Create<N>(text, new()
            {
                ("NumberStyles", numberStyles),
                ("IFormatProvider", provider),
            });
        }
    }
}
#endif