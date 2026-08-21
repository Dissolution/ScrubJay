#if NET7_0_OR_GREATER

using System.Globalization;

namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on static <see cref="ISpanParsable{TSelf}"/>, <see cref="IParsable{TSelf}"/>, and <see cref="INumberBase{TSelf}"/> types.
/// </summary>
[PublicAPI]
public static class ParseableExtensions
{
    extension<P>(P)
        where P : ISpanParsable<P>
    {
        public static Result<P> TryParse(
            scoped text text,
            IFormatProvider? provider = null)
        {
            if (P.TryParse(text, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse '{text}' into a {typeof(P).Alias} value", nameof(text));
        }
    }

    extension<P>(P)
        where P : IParsable<P>
    {
        public static Result<P> TryParse(
            [NotNullWhen(true)] string? str,
            IFormatProvider? provider = null)
        {
            if (P.TryParse(str, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse \"{str}\" into a {typeof(P).Alias} value", nameof(str));
        }
    }

    extension<N>(N)
        where N : INumberBase<N>
    {
        public static Result<N> TryParse(
            scoped text text,
            NumberStyles style = NumberStyles.Number,
            IFormatProvider? provider = null)
        {
            if (N.TryParse(text, style, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse '{text}' into a {typeof(N).Alias} value", nameof(text));
        }

        public static Result<N> TryParse(
            [NotNullWhen(true)] string? str,
            NumberStyles style = NumberStyles.Number,
            IFormatProvider? provider = null)
        {
            if (N.TryParse(str, style, provider, out var value))
                return value;
            return new ArgumentException($"Could not parse \"{str}\" into a {typeof(N).Alias} value", nameof(str));
        }
    }
}
#endif