#pragma warning disable CS8764

namespace ScrubJay.Errors;

public partial class Ex
{
    public static ArgException Arg<T>(
        in T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        return new ArgException(arg, message);
    }

    public static ArgException Arg(object? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        var arg = Argument.Capture(argument, argumentName);
        return new ArgException(arg, message);
    }
}