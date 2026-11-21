using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors;

public static partial class Ex
{
    public static ArgRangeException ArgRange<T>(
        T? argument,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => ArgRangeException.Create<T>(argument, additionalInfo, innerException, argumentName);

    public static ArgRangeException ArgRange(
        Argument argument,
        string? additionalInfo = null,
        Exception? innerException = null)
        => ArgRangeException.Create(argument, additionalInfo, innerException);
}