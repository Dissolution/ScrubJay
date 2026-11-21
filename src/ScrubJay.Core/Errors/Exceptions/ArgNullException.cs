#pragma warning disable CA1032

using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;
using ScrubJay.Text;

namespace ScrubJay.Errors.Exceptions;

/// <summary>
/// An enhanced <see cref="ArgumentNullException"/>.
/// </summary>
[PublicAPI]
public class ArgNullException : ArgumentNullException
{
    [StackTraceHidden]
    public static ArgNullException Create(
        Argument argument,
        string? additionalInfo = null,
        Exception? innerException = null)
    {
        using Interpolator interpolator = $"Argument was null";
        if (!string.IsNullOrEmpty(additionalInfo))
        {
            interpolator.Write(": ");
            interpolator.Write(additionalInfo);
        }

        return new ArgNullException(argument, interpolator, innerException);
    }

    [StackTraceHidden]
    public static ArgNullException Create<T>(
        T? argument = default,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        => Create(Argument.Capture(argument, argumentName), additionalInfo, innerException);


    public Argument Argument { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionAccess.RefMessage(this) ?? string.Empty;

    public ArgNullException(
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