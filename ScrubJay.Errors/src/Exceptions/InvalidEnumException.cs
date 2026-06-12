using System.ComponentModel;

namespace ScrubJay.Errors.Exceptions;

[PublicAPI]
public sealed class InvalidEnumException : InvalidEnumArgumentException
{
    public Argument Argument { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionFields.RefMessageField(this) ?? "";

    public InvalidEnumException(Argument argument, string? message = null, Exception? innerException = null)
        : base()
    {
        Argument = argument;
        ExceptionFields.RefMessageField(this) = message;
        ExceptionFields.RefInnerExceptionField(this) = innerException;
    }
}