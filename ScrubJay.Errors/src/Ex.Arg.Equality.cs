namespace ScrubJay.Errors;

public partial class Ex
{
    public static ArgException ArgNotEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        var message = TextBuilder.Rent()
            .Append("Argument ")
            .Render(arg)
            .Append(" was not equal to ")
            .Render(expected)
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgException(arg, message);
    }

    public static ArgException ArgEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        var message = TextBuilder.Rent()
            .Append("Argument ")
            .Render(arg)
            .Append(" was equal to ")
            .Render(expected)
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgException(arg, message);
    }
}