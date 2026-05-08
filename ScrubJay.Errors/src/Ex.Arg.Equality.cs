namespace ScrubJay.Errors;

partial class Ex
{
    internal static ArgException ArgNotEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        var message = TextBuilder.Rent()
            .Append($"Argument {arg:@} was not equal to {expected:@}")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgException(arg, message);
    }

    internal static ArgException ArgEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        var message = TextBuilder.Rent()
            .Append($"Argument {arg:@} was equal to {expected:@}")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgException(arg, message);
    }
}