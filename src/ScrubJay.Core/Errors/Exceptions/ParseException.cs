using ScrubJay.Errors.Utilities;
using ScrubJay.Text;

namespace ScrubJay.Errors.Exceptions;

[PublicAPI]
public class ParseException : FormatException
{
    [StackTraceHidden]
    public static ParseException Create<T>(
        string? input,
        string? additionalInfo = null,
        Exception? innerException = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        using Interpolator message = $"Could not parse \"{input}\" into a {typeof(T).Alias} value";
        if (!string.IsNullOrEmpty(additionalInfo))
        {
            message.Write(": ");
            message.Write(additionalInfo);
        }

        return new ParseException(input, typeof(T), message, innerException);
    }

    [StackTraceHidden]
    public static ParseException Create<T>(
        scoped text input,
        string? additionalInfo = null,
        Exception? innerException = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Create<T>(input.ToString(), additionalInfo, innerException);


    public string? InputString { get; }

    public Type? DestinationType { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionAccess.RefMessage(this) ?? string.Empty;

    public ParseException(
        string? inputString,
        Type? destinationType,
        string? message = null,
        Exception? innerException = null)
        : base()
    {
        this.InputString = inputString;
        this.DestinationType = destinationType;
        ExceptionAccess.RefMessage(this) = message;
        ExceptionAccess.RefInnerException(this) = innerException;
    }

    public override string ToString()
    {
        return $"""
            {GetType().Alias}> "{InputString}" -> {DestinationType.Alias}
                {Message}
            """;
    }
}