#pragma warning disable CA1032

using System.ComponentModel;
using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;
using ScrubJay.Text;

namespace ScrubJay.Errors.Exceptions;

[PublicAPI]
public sealed class InvalidEnumException : InvalidEnumArgumentException
{
    [StackTraceHidden]
    public static InvalidEnumException Create(
        Argument argument,
        string? additionalInfo = null,
        Exception? innerException = null)
    {
        using Interpolator message = $"{argument.Value} is not a valid {argument.Type.Alias} Enum value";
        if (!string.IsNullOrEmpty(additionalInfo))
        {
            message.Write(": ");
            message.Write(additionalInfo);
        }

        return new InvalidEnumException(argument, message, innerException);
    }

    [StackTraceHidden]
    public static InvalidEnumException Create<E>(
        in E @enum,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(@enum))]
        string? enumName = null)
        where E : struct, Enum
        => Create(Argument.Capture(@enum, enumName), additionalInfo, innerException);


    public Argument Argument { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionAccess.RefMessage(this) ?? string.Empty;

    public InvalidEnumException(
        Argument argument,
        string? message = null,
        Exception? innerException = null)
        : base()
    {
        Argument = argument;
        ExceptionAccess.RefParamName(this) = argument.Name;
        ExceptionAccess.RefMessage(this) = message;
        ExceptionAccess.RefInnerException(this) = innerException;
    }
}