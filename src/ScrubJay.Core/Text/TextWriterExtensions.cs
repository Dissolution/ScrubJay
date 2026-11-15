using ScrubJay.Generics;

namespace ScrubJay.Text;

public delegate void TextWrite<W>(W writer)
    where W : ITextWriter, allows ref struct;

public delegate void TextWriteValue<W, T>(W writer, T value)
    where W : ITextWriter, allows ref struct;





[PublicAPI]
public static class TextWriterExtensions
{


    extension<W>(W writer)
        where W : ITextWriter, allows ref struct
    {
        public W Append(char ch)
        {
            writer.Write(ch);
            return writer;
        }

        public W Append(string? str)
        {
            writer.Write(str);
            return writer;
        }

        public W Append(text txt)
        {
            writer.Write(txt);
            return writer;
        }

        public W Append<T>(T? value)
        {
            if (value is null)
            {
                return writer;
            }

            string? str;
            if (value is IFormattable)
            {
                // If the value can format itself directly into our buffer, do so.
                if (value is ISpanFormattable)
                {
                    int hint = 16;
                    int charsWritten;

                    var destination = writer.GetSpan(hint);
                    while (!((ISpanFormattable)value).TryFormat(destination, out charsWritten, default, default))
                    {
                        hint *= 2;
                        destination = writer.GetSpan(hint);
                    }

                    writer.Advance(charsWritten);
                    return writer;
                }

                str = ((IFormattable)value).ToString(default, default);
            }
            else
            {
                str = value.ToString();
            }

            writer.Write(str);
            return writer;
        }


        public W NewLine()
        {
            writer.Write(Environment.NewLine);
            return writer;
        }

#if NET6_0_OR_GREATER
        [OverloadResolutionPriority(100)]
        public W Format<SF>(SF? value, text format = default, IFormatProvider? provider = default, Constrain.AllowsRefStruct.HasISpanFormattable<SF> _ = default)
            where SF : ISpanFormattable
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
        {
            if (value is null)
                return writer;

            int hint = 16;
            int charsWritten;

            var destination = writer.GetSpan(hint);
            while (!value.TryFormat(destination, out charsWritten, format, provider))
            {
                hint *= 2;
                destination = writer.GetSpan(hint);
            }

            writer.Advance(charsWritten);
            return writer;
        }
#endif

        [OverloadResolutionPriority(90)]
        public W Format<F>(F? value, string? format = default, IFormatProvider? provider = default, Constrain.AllowsRefStruct.HasIFormattable<F> _ = default)
            where F : IFormattable
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
        {
            if (value is null)
                return writer;

            string? str = value.ToString(format, provider);
            writer.Write(str);
            return writer;
        }

        [OverloadResolutionPriority(70)]
        public W Format<T>(T? value, string? format = default, IFormatProvider? provider = default)
        {
            if (value is null)
            {
                return writer;
            }

            string? str;
            if (value is IFormattable)
            {
                // If the value can format itself directly into our buffer, do so.
                if (value is ISpanFormattable)
                {
                    int hint = 16;
                    int charsWritten;

                    var destination = writer.GetSpan(hint);
                    while (!((ISpanFormattable)value).TryFormat(destination, out charsWritten, format, provider))
                    {
                        hint *= 2;
                        destination = writer.GetSpan(hint);
                    }

                    writer.Advance(charsWritten);
                    return writer;
                }

                str = ((IFormattable)value).ToString(format, provider);
            }
            else
            {
                str = value.ToString();
            }

            writer.Write(str);
            return writer;
        }


        public W IfNull<T>(
            T? value,
            TextWrite<W>? writeIfNull,
            TextWriteValue<W, T>? writeIfNotNull = null)
        {
            if (value is null)
            {
                writeIfNull?.Invoke(writer);
            }
            else
            {
                writeIfNotNull?.Invoke(writer, value);
            }
            return writer;
        }

        public W IfNotNull<T>(
            T? value,
            TextWriteValue<W, T>? writeIfNotNull,
            TextWrite<W>? writeIfNull = null)
        {
            if (value is not null)
            {
                writeIfNotNull?.Invoke(writer, value);
            }
            else
            {
                writeIfNull?.Invoke(writer);
            }
            return writer;
        }
    }
}