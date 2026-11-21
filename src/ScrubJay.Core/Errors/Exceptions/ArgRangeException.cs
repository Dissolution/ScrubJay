#pragma warning disable CA1032

using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;
using ScrubJay.Text;

namespace ScrubJay.Errors.Exceptions;

/// <summary>
/// An enhanced <see cref="ArgumentOutOfRangeException"/>.
/// </summary>
[PublicAPI]
public class ArgRangeException : ArgumentOutOfRangeException
{
    [StackTraceHidden]
    public static ArgRangeException Create<T>(
        T? argument = default,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        using Interpolator message = $"Argument was out of range";

        if (!string.IsNullOrEmpty(additionalInfo))
        {
            message.Write(": ");
            message.Write(additionalInfo);
        }

        return new ArgRangeException(Argument.Capture(argument, argumentName), message, innerException);
    }


    public Argument Argument { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionAccess.RefMessage(this) ?? string.Empty;

    public ArgRangeException(
        Argument argument,
        string? message = null,
        Exception? innerException = null)
        : base()
    {
        this.Argument = argument;
        ExceptionAccess.RefActualValue(this) = argument.Value;
        ExceptionAccess.RefParamName(this) = argument.Name;
        ExceptionAccess.RefMessage(this) = message;
        ExceptionAccess.RefInnerException(this) = innerException;
    }

    public override string ToString()
    {
        return $"""
            {GetType().Alias} - {Argument}
                {Message}
            """;
    }
}