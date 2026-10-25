namespace ScrubJay.Functional;

[PublicAPI]
public interface ITrySpanFormattable : ISpanFormattable, IFormattable
{
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, text format, IFormatProvider? provider)
        => TryFormat(destination, format, provider).IsOk(out charsWritten);

    Result<int, Exception> TryFormat(Span<char> destination, text format = default, IFormatProvider? provider = default);
}