namespace ScrubJay.Functional;

[PublicAPI]
public interface ITrySpanParsable<S> : ISpanParsable<S>
    where S : ITrySpanParsable<S>
{
    static S IParsable<S>.Parse(string s, IFormatProvider? provider)
        => S.TryParse(s, provider).OkOrThrow();

    static bool IParsable<S>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out S result)
        => S.TryParse(s, provider).IsOk(out result);

    static abstract Result<S, Exception> TryParse([NotNullWhen(true)] string? str, IFormatProvider? provider = default);


    static S ISpanParsable<S>.Parse(text s, IFormatProvider? provider)
        => S.TryParse(s, provider).OkOrThrow();

    static bool ISpanParsable<S>.TryParse(text s, IFormatProvider? provider, [MaybeNullWhen(false)] out S result)
        => S.TryParse(s, provider).IsOk(out result);

    static abstract Result<S, Exception> TryParse(text txt, IFormatProvider? provider = default);
}