namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class FormattableExtensions
{
    extension<T>(T value)
        where T : ISpanFormattable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
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
}