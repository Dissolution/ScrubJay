namespace ScrubJay.Errors.Parsing;

[PublicAPI]
public interface ITrySpanParsable<T> : ISpanParsable<T>, ITryParsable<T>
    where T : ITrySpanParsable<T>
{
#if NET7_0_OR_GREATER
    static T ISpanParsable<T>.Parse(text text, IFormatProvider? provider) => T.TryParse(text, provider).OkOrThrow();

    static bool ISpanParsable<T>.TryParse(text text, IFormatProvider? provider, [MaybeNullWhen(false)] out T result)
    {
        if (T.TryParse(text, provider).IsOk(out result))
            return true;

        result = default;
        return false;
    }

    static abstract Result<T> TryParse(scoped text text, IFormatProvider? provider = null);
#endif
}

[PublicAPI]
public interface ITryParsable<T> : IParsable<T>
    where T : ITryParsable<T>
{
#if NET7_0_OR_GREATER
    static T IParsable<T>.Parse(string str, IFormatProvider? provider) => T.TryParse(str, provider).OkOrThrow();

    static bool IParsable<T>.TryParse([NotNullWhen(true)] string? str, IFormatProvider? provider, [MaybeNullWhen(false)] out T result)
    {
        if (T.TryParse(str, provider).IsOk(out result))
            return true;

        result = default;
        return false;
    }

    static abstract Result<T> TryParse([NotNullWhen(true)] string? str, IFormatProvider? provider = null);
#endif
}