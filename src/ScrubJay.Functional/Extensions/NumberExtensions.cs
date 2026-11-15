using System.Globalization;

namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class NumberExtensions
{
    extension<N>(N)
        where N : INumberBase<N>
    {
        public static Option<N> AsParsed(string? str, NumberStyles numberStyles = NumberStyles.None, IFormatProvider? provider = null)
        {
            if (N.TryParse(str, numberStyles, provider, out var number))
                return number;
            return default;
        }

        public static Option<N> AsParsed(text txt, NumberStyles numberStyles = NumberStyles.None, IFormatProvider? provider = null)
        {
            if (N.TryParse(txt, numberStyles, provider, out var number))
                return number;
            return default;
        }

        public static Result<N, Exception> TryParse(string? str, NumberStyles numberStyles = NumberStyles.None, IFormatProvider? provider = null)
        {
            try
            {
                return N.Parse(str!, numberStyles, provider);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static Result<N, Exception> TryParse(text txt, NumberStyles numberStyles = NumberStyles.None, IFormatProvider? provider = null)
        {
            try
            {
                return N.Parse(txt, numberStyles, provider);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}