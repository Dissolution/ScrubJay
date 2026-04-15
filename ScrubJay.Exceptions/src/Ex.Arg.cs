#pragma warning disable CS8764

namespace ScrubJay.Exceptions;

public partial class Ex
{
    public static ArgumentException Arg<T>(
        T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        var arg = Argument.Capture<T>(argument, argumentName);
        return new ArgException(arg, message);
    }
}