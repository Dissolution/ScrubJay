namespace ScrubJay.Generics;

public static partial class Anything
{
   
    
    [OverloadResolutionPriority(100)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? Format<T>(T? value, string? format, IFormatProvider? provider = default)
    {
        if (value is null)
            return null;

        if (value is IFormattable)
        {
            return ((IFormattable)value).ToString(format, provider);
        }

        return value.ToString() ?? "";
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(75)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? Format<T>(T? value, string? format, IFormatProvider? provider = default, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        throw new NotImplementedException();
    }
#endif

    [OverloadResolutionPriority(100)]
    public static bool TryFormat<T>(T? value, Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = default)
    {
        if (value is null)
        {
            charsWritten = 0;
            return true;
        }

        text formatted;

        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                return ((ISpanFormattable)value).TryFormat(destination, out charsWritten, format, provider);
            }
#endif

            formatted = ((IFormattable)value).ToString(format.ToString(), provider);
        }
        else
        {
            formatted = value.ToString();
        }

        if (formatted.TryCopyTo(destination))
        {
            charsWritten = formatted.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

    [OverloadResolutionPriority(90)]
    public static bool TryFormat<T>(T? value, Span<char> destination, out int charsWritten, string? format = default, IFormatProvider? provider = default)
    {
        if (value is null)
        {
            charsWritten = 0;
            return true;
        }

        text formatted;

        if (value is IFormattable)
        {
#if NET6_0_OR_GREATER
            if (value is ISpanFormattable)
            {
                return ((ISpanFormattable)value).TryFormat(destination, out charsWritten, format, provider);
            }
#endif

            formatted = ((IFormattable)value).ToString(format, provider);
        }
        else
        {
            formatted = value.ToString();
        }

        if (formatted.TryCopyTo(destination))
        {
            charsWritten = formatted.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(75)]
    public static bool TryFormat<T>(T? value, Span<char> destination, out int charsWritten, text format = default, IFormatProvider? provider = default, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        throw new NotImplementedException();
    }
#endif
}