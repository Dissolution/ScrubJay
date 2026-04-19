
// ReSharper disable MergeCastWithTypeCheck

namespace ScrubJay.Text.Building;

public partial class TextBuilder
{
    public TextBuilder Format<T>(T? value)
    {
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, default, null))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
            }
            else
#endif
            {
                Write(((IFormattable)value).ToString(null, null));
            }
        }
        else if (value is not null)
        {
            Write(value.ToString());
        }

        return this;
    }
    
    public TextBuilder Format<T>(T? value, string? format)
    {
//        if (format is [RENDER.FORMAT])
//        {
//            return Render<T>(value);
//        }
        
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, format, null))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
            }
            else
#endif
            {
                Write(((IFormattable)value).ToString(format, null));
            }
        }
        else if (value is not null)
        {
            Write(value.ToString());
        }

        return this;
    }

    public TextBuilder Format<T>(T? value, string? format, IFormatProvider? formatProvider)
    {
        // do not check for render, this is the overload to ignore
        
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, format, formatProvider))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
            }
            else
#endif
            {
                Write(((IFormattable)value).ToString(format, formatProvider));
            }
        }
        else if (value is not null)
        {
            Write(value.ToString());
        }

        return this;
    }

    public TextBuilder Format<T>(T? value, scoped text format)
    {
//        if (format is [RENDER.FORMAT])
//        {
//            return Render<T>(value);
//        }
        
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, format, null))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
            }
            else
#endif
            {
                Write(((IFormattable)value).ToString(format.ToString(), null));
            }
        }
        else if (value is not null)
        {
            Write(value.ToString());
        }

        return this;
    }

    public TextBuilder Format<T>(T? value, scoped text format, IFormatProvider? formatProvider)
    {
        // do not check for render, this is the overload to ignore
        
        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                int charsWritten;
                while (!((ISpanFormattable)value).TryFormat(Available, out charsWritten, format, formatProvider))
                {
                    GrowBy(16);
                }

                _position += charsWritten;
            }
            else
#endif
            {
                Write(((IFormattable)value).ToString(format.ToString(), formatProvider));
            }
        }
        else if (value is not null)
        {
            Write(value.ToString());
        }

        return this;
    }

#if NET9_0_OR_GREATER

    private TextBuilder CallFormat<T>(T? value, string? format, IFormatProvider? formatProvider)
        where T : allows ref struct
    {
        Emit.Ldarg_0(); // this
        Emit.Ldarg(nameof(value));
        Emit.Ldarg(nameof(format));
        Emit.Ldarg(nameof(formatProvider));
        Emit.Call(new MethodRef(
                typeof(TextBuilder),
                nameof(Format), 
                1, 
                [typeof(T), typeof(string), typeof(IFormatProvider)])
            .MakeGenericMethod(typeof(T)));
        return Return<TextBuilder>();
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // ReSharper disable once MethodOverloadWithOptionalParameter
    public TextBuilder Format<T>(
        in T? value, 
        string? format = null,
        IFormatProvider? formatProvider = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
//        if (format is [RENDER.FORMAT])
//        {
//            return Render<T>(value);
//        }

        if (typeof(T).IsByRef)
        {
            return Append(Any.ToString<T>(in value, format, formatProvider, _));
        }

        // we cannot defer to Format<T> as we have the `allows ref struct` constraint on our `T`
        // even though we know that this value is not a ref struct
        // The only way to bypass is to abuse Emission
        return CallFormat(value, format, formatProvider);
    }
#endif
}