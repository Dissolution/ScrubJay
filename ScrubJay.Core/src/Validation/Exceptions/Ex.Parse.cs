using ScrubJay.Parsing;
using ScrubJay.Rendering.Rendition3;

namespace ScrubJay.Validation;

partial class Ex
{
    private static string GetParseExceptionMessage(scoped text input, Type? destType, string? info)
    {
        return TextBuilder.New
            .Append($"Could not parse \"{input}\" into a {destType:@}")
            .AppendOptionalInfo(info)
            .ToStringAndDispose();
    }

    private static string GetParseExceptionMessage(scoped text input, Type? destType, ref InterpolatedTextBuilder info)
    {
        return TextBuilder.New
            .Append($"Could not parse \"{input}\" into a {destType:@}")
            .AppendOptionalInfo(ref info)
            .ToStringAndDispose();
    }

    private static string GetParseExceptionMessage(string? input, Type? destType, string? info)
    {
        return TextBuilder.New
            .Append("Could not parse ")
            .IfNotNull(input,
                static (tb, n) => tb.Append('"').Append(n).Append('"'),
                static tb => tb.Append("〈null〉"))
            .Append(" into a ")
            .Render(destType)
            .AppendOptionalInfo(info)
            .ToStringAndDispose();
    }

    private static string GetParseExceptionMessage(string? input, Type? destType, ref InterpolatedTextBuilder info)
    {
        return TextBuilder.New
            .Append("Could not parse ")
            .IfNotNull(input,
                static (tb, n) => tb.Append('"').Append(n).Append('"'),
                static tb => tb.Append("〈null〉"))
            .Append(" into a ")
            .Render(destType)
            .AppendOptionalInfo(ref info)
            .ToStringAndDispose();
    }


    public static ParseException Parse(scoped text input, Type? destType, string? info = default)
    {
        var message = GetParseExceptionMessage(input, destType, info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = destType,
        };
    }

    public static ParseException Parse(scoped text input, Type? destType, ref InterpolatedTextBuilder info)
    {
        var message = GetParseExceptionMessage(input, destType, ref info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = destType,
        };
    }

    public static ParseException Parse(string? input, Type? destType, string? info = default)
    {
        var message = GetParseExceptionMessage(input, destType, info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = destType,
        };
    }

    public static ParseException Parse(string? input, Type? destType, ref InterpolatedTextBuilder info)
    {
        var message = GetParseExceptionMessage(input, destType, ref info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = destType,
        };
    }




    public static ParseException Parse<T>(
        scoped text input,
        string? info = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetParseExceptionMessage(input, typeof(T), info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = typeof(T),
        };
    }

    public static ParseException Parse<T>(
        scoped text input,
        ref InterpolatedTextBuilder info)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetParseExceptionMessage(input, typeof(T), ref info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = typeof(T),
        };
    }
    
    public static ParseException Parse<T>(
        string? input,
        string? info = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetParseExceptionMessage(input, typeof(T), info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = typeof(T),
        };
    }

    public static ParseException Parse<T>(
        string? input,
        ref InterpolatedTextBuilder info)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetParseExceptionMessage(input, typeof(T), ref info);
        return new ParseException(message)
        {
            InputText = input.AsString(),
            DestinationType = typeof(T),
        };
    }
}