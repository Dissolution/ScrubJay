#pragma warning disable CS8764

namespace ScrubJay.Exceptions;

public partial class Ex
{
    public static ArgException Arg<T>(
        in T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        return new ArgException(arg, message);
    }

#if NET9_0_OR_GREATER
    public static ArgException Arg<T>(
        in T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null,
        // ReSharper disable once MethodOverloadWithOptionalParameter
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        var arg = Argument.Capture<T>(in argument, argumentName, _);
        return new ArgException(arg, message);
    }
#endif

    public static ArgException Arg(object? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        var arg = Argument.Capture(argument, argumentName);
        return new ArgException(arg, message);
    }
}