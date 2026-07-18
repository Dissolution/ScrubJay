using ScrubJay.Universal;
// ReSharper disable MergeCastWithTypeCheck

namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class SpanWriterExtensions
{
    extension(ref SpanWriter<char> writer)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryWrite(string? str) => writer.TryWrite(str.AsSpan());

        public bool TryWrite<T>(T? value)
        {
            if (value is null)
                return true;

            if (value is IFormattable)
            {
#if NET6_0_OR_GREATER
                if (value is ISpanFormattable)
                {
                    if (((ISpanFormattable)value).TryFormat(writer.Available, out int charsWritten, default, default))
                    {
                        writer._position += charsWritten;
                        return true;
                    }
                    return false;
                }
#endif

                return writer.TryWrite(((IFormattable)value).ToString(default, default));
            }

            return writer.TryWrite(value.ToString());
        }

#if NET9_0_OR_GREATER
        public bool TryWrite<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
            where T : allows ref struct
        {
            return writer.TryWrite(Any.ToString<T>(in value));
        }
#endif

        public bool TryWrite<T>(T? value, string? format, IFormatProvider? provider = null)
        {
            if (value is null)
                return true;

            if (value is IFormattable)
            {
#if NET6_0_OR_GREATER
                if (value is ISpanFormattable)
                {
                    if (((ISpanFormattable)value).TryFormat(writer.Available, out int charsWritten,
                        format, provider))
                    {
                        writer._position += charsWritten;
                        return true;
                    }
                    return false;
                }
#endif

                return writer.TryWrite(((IFormattable)value).ToString(format, provider));
            }

            return writer.TryWrite(value.ToString());
        }

        public bool TryWrite<T>(T? value, text format, IFormatProvider? provider = null)
        {
            if (value is null)
                return true;

            if (value is IFormattable)
            {
#if NET6_0_OR_GREATER
                if (value is ISpanFormattable)
                {
                    if (((ISpanFormattable)value).TryFormat(writer.Available, out int charsWritten,
                        format, provider))
                    {
                        writer._position += charsWritten;
                        return true;
                    }
                    return false;
                }
#endif

                return writer.TryWrite(((IFormattable)value).ToString(format.ToString(), provider));
            }

            return writer.TryWrite(value.ToString());
        }
    }
}