using System.Globalization;

namespace ScrubJay.Functional;

[PublicAPI]
public static class FunctionalExtensions
{
    extension<E, T>(ref E enumerator)
        where E : struct, IEnumerator<T>
    {
        public Option<T> Next()
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            return default;
        }
    }
    
    extension<E, T>(E enumerator)
        where E : class, IEnumerator<T>
    {
        public Option<T> Next()
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            return default;
        }
    }

    extension<T>(T)
        where T : IParsable<T>
    {
        public static Option<T> AsParsed(string? str, IFormatProvider? provider = null)
        {
            if (T.TryParse(str, provider, out var value))
                return value;
            return default;
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
    }

    extension<T>(T value)
        where T : ISpanFormattable
    {
        public Option<int> TryFormat(Span<char> destination, text format = default, IFormatProvider? provider = default)
        {
            if (value.TryFormat(destination, out int charsWritten, format, provider))
            {
                return charsWritten;
            }
            return default;
        }
    }

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