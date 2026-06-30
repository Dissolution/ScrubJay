using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;

namespace ScrubJay.Errors.Exceptions;

/// <summary>
/// An enhanced <see cref="ArgumentNullException"/>.
/// </summary>
[PublicAPI]
public class ArgNullException : ArgumentNullException, IScrubJayException<ArgNullException>
{
#region Throw / Create
    [DoesNotReturn]
    public static void Throw<T>(
        in T? argument,
        string? info = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Create(in argument, info, innerException, argumentName);
    }

    [DoesNotReturn]
    public static void Throw(
        ArgumentInfo argumentInfo,
        string? info = null,
        Exception? innerException = null)
    {
        throw Create(argumentInfo, info, innerException);
    }

    public static ArgNullException Create<T>(
        in T? argument,
        string? info = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = ArgumentInfo.Capture<T>(in argument, argumentName);
        return Create(arg, info, innerException);
    }

    public static ArgNullException Create(
        ArgumentInfo argumentInfo,
        string? info = null,
        Exception? innerException = null)
    {
        DefaultInterpolatedStringHandler builder
            = $"ArgNullException - \"{argumentInfo.Name}\": {argumentInfo.Type}";
        if (string.IsNullOrEmpty(info))
        {
            builder.AppendLiteral(" was null");
        }
        else
        {
            builder.AppendLiteral(" ");
            builder.AppendLiteral(info!);
        }
        var message = builder.ToStringAndClear();

        return new ArgNullException(argumentInfo, message, innerException);
    }
#endregion

    public ArgumentInfo ArgumentInfo { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionAccess.RefMessageField(this) ?? string.Empty;

    public ArgNullException(
        ArgumentInfo argument,
        string? message = null,
        Exception? innerException = null)
        : base(paramName: argument.Name, message: message)
    {
        /* Note: is there is no ArgumentNullException that takes paramName, message, and innerException.
         * All of them that take message will replace it if it is null.
         * So we use paramName + message and then override message + innerException.
         */
        ArgumentInfo = argument;
        ExceptionAccess.RefMessageField(this) = message;
        if (innerException is not null)
        {
            ExceptionAccess.RefInnerExceptionField(this) = innerException;
        }
    }
}