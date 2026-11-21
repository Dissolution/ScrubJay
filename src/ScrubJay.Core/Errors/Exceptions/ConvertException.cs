using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;
using ScrubJay.Text;

namespace ScrubJay.Errors.Exceptions;

[PublicAPI]
public class ConvertException : InvalidCastException
{
    [StackTraceHidden]
    public static ConvertException Create(
        Argument sourceArgument,
        Type? destinationType,
        string? additionalInfo = null,
        Exception? innerException = null)
    {
        using Interpolator message = $"Could not convert from {sourceArgument} to {destinationType.Alias}";
        if (!string.IsNullOrEmpty(additionalInfo))
        {
            message.Write(": ");
            message.Write(additionalInfo);
        }

        return new ConvertException(sourceArgument, destinationType, message, innerException);
    }

    public Argument SourceArgument { get; }

    public Type? DestinationType { get; }

    public override string Message => ExceptionAccess.RefMessage(this) ?? string.Empty;

    public ConvertException(
        Argument sourceArgument,
        Type? destinationType,
        string? message = null,
        Exception? innerException = null)
        : base()
    {
        this.SourceArgument = sourceArgument;
        this.DestinationType = destinationType;
        ExceptionAccess.RefMessage(this) = message;
        ExceptionAccess.RefInnerException(this) = innerException;
    }

    public override string ToString()
    {
        return $"""
            {GetType().Alias}> {SourceArgument} -> {DestinationType.Alias}
                {Message}
            """;
    }
}