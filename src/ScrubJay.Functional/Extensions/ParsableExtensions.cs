namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class ParsableExtensions
{
    extension<T>(T)
        where T : IParsable<T>
    {
        public static Option<T> AsParsed(string? str, IFormatProvider? provider = null)
        {
            if (T.TryParse(str, provider, out var value))
                return value;
            return default;
        }

        public static Result<T, Exception> TryParse(string? str, IFormatProvider? provider = null)
        {
            if (str is null)
                return new ArgumentNullException(nameof(str));

            try
            {
                return T.Parse(str, provider);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }

    extension<T>(T)
        where T : ISpanParsable<T>
    {
        public static Option<T> AsParsed(text txt, IFormatProvider? provider = null)
        {
            if (T.TryParse(txt, provider, out var value))
                return value;
            return default;
        }

        public static Result<T, Exception> TryParse(text txt, IFormatProvider? provider = null)
        {
            try
            {
                return T.Parse(txt, provider);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}