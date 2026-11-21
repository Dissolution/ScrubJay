using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors;

public static partial class Ex
{
    public static ArgException Arg<T>(
        T? argument,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => ArgException.Create<T>(argument, additionalInfo, innerException, argumentName);

    public static ArgException Arg(
        Argument argument,
        string? additionalInfo = null,
        Exception? innerException = null)
        => ArgException.Create(argument, additionalInfo, innerException);
}