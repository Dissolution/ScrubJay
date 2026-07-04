using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors;

partial class Ex
{
    public static ArgRangeException ArgRange(
        object? argument,
        string? info = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        => ArgRangeException.Create(argument, info, innerException, argumentName);

    public static ArgRangeException ArgRange<T>(
        in T? argument,
        string? info = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => ArgRangeException.Create<T>(in argument, info, innerException, argumentName);
}