using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;
using ScrubJay.Text;

namespace ScrubJay.Errors.Exceptions;

/// <summary>
/// An enhanced <see cref="ArgumentException"/>.
/// </summary>
[PublicAPI]
public class ArgException : ArgumentException
{
    [StackTraceHidden]
    public static ArgException Create(
        Argument argument,
        string? additionalInfo = null,
        Exception? innerException = null)
    {
        using Interpolator interpolator = $"Argument was invalid";
        if (!string.IsNullOrEmpty(additionalInfo))
        {
            interpolator.Write(": ");
            interpolator.Write(additionalInfo);
        }

        return new ArgException(argument, interpolator, innerException);
    }

    [StackTraceHidden]
    public static ArgException Create<T>(
        T? argument = default,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Create(Argument.Capture(argument, argumentName), additionalInfo, innerException);


    public Argument Argument { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionAccess.RefMessage(this) ?? string.Empty;

    public ArgException(
        Argument argument,
        string? message = null,
        Exception? innerException = null)
        : base()
    {
        this.Argument = argument;
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