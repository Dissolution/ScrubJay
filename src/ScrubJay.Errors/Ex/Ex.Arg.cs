using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors;

partial class Ex
{
    public static ArgException Arg<T>(
        in T? argument,
        string? info = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => ArgException.Create<T>(in argument, info, innerException, argumentName);

    public static ArgException Arg(
        ArgumentInfo argumentInfo,
        string? info = null,
        Exception? innerException = null)
        => ArgException.Create(argumentInfo, info, innerException);
}