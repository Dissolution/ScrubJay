#if NET7_0_OR_GREATER

namespace ScrubJay.Functional;

[PublicAPI]
public interface ITryParsable<TSelf> : IParsable<TSelf>
    where TSelf : ITryParsable<TSelf>?
{
    static TSelf IParsable<TSelf>.Parse(
        [NotNull] string str, 
        IFormatProvider? provider) 
        => TSelf.TryParse(str, provider).OkOrThrow();

    static bool IParsable<TSelf>.TryParse(
        [AllowNull, NotNullWhen(true)] string? str,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out TSelf parsed)
        => TSelf.TryParse(str, provider).IsOk(out parsed);

    static abstract Result<TSelf> TryParse([AllowNull, NotNullWhen(true)] string? str, IFormatProvider? provider = null);
}

[PublicAPI]
public interface ITrySpanParsable<TSelf> : ISpanParsable<TSelf>, 
    ITryParsable<TSelf>, IParsable<TSelf>
    where TSelf : ITrySpanParsable<TSelf>?
{
    static TSelf ISpanParsable<TSelf>.Parse(text text, IFormatProvider? provider) 
        => TSelf.TryParse(text, provider).OkOrThrow();

    static bool ISpanParsable<TSelf>.TryParse(text text, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf parsed)
        => TSelf.TryParse(text, provider).IsOk(out parsed);

    static Result<TSelf> ITryParsable<TSelf>.TryParse([NotNullWhen(true)] string? str, IFormatProvider? provider)
        => TSelf.TryParse(str.AsSpan(), provider);

    static abstract Result<TSelf> TryParse(text text, IFormatProvider? provider = null);
}

#endif