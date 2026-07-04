using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;

namespace ScrubJay.Errors.Exceptions;

/// <summary>
/// An enhanced <see cref="ArgumentOutOfRangeException"/>.
/// </summary>
[PublicAPI]
public class ArgRangeException : ArgumentOutOfRangeException, IArgumentException<ArgRangeException>
{
#region Throw / Create
    [DoesNotReturn]
    [StackTraceHidden]
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
    [StackTraceHidden]
    public static void Throw(
        ArgumentInfo argumentInfo,
        string? info = null,
        Exception? innerException = null)
    {
        throw Create(argumentInfo, info, innerException);
    }

    [StackTraceHidden]
    public static ArgRangeException Create<T>(
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
    
    [StackTraceHidden]
    public static ArgRangeException Create<T>(
        Argument<T> argument,
        string? info = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        DefaultInterpolatedStringHandler builder = $"ArgRangeException - {argument}";
        if (string.IsNullOrEmpty(info))
        {
            builder.AppendLiteral(" was out of range");
        }
        else
        {
            builder.AppendLiteral(" ");
            builder.AppendLiteral(info!);
        }
        var message = builder.ToStringAndClear();

        return new ArgRangeException(argument, argument.Value, message, innerException);
    }

    [StackTraceHidden]
    public static ArgRangeException Create(
        ArgumentInfo argumentInfo,
        string? info = null,
        Exception? innerException = null)
    {
        DefaultInterpolatedStringHandler builder = $"ArgRangeException - {argumentInfo}";
        if (string.IsNullOrEmpty(info))
        {
            builder.AppendLiteral(" was out of range");
        }
        else
        {
            builder.AppendLiteral(" ");
            builder.AppendLiteral(info!);
        }
        var message = builder.ToStringAndClear();

        return new ArgRangeException(argumentInfo, message, innerException);
    }
#endregion


    public ArgumentInfo ArgumentInfo { get; }

    public override string Message => ExceptionAccess.RefMessageField(this) ?? string.Empty;

    private ArgRangeException(
        ArgumentInfo argumentInfo,
        object? argument,
        string? message = null,
        Exception? innerException = null)
        : base(paramName: argumentInfo.Name, actualValue: argument, message: message)
    {
        ArgumentInfo = argumentInfo;
        ExceptionAccess.RefMessageField(this) = message;
        ExceptionAccess.RefInnerExceptionField(this) = innerException;
    }
    
    public ArgRangeException(
        ArgumentInfo argument,
        string? message = null,
        Exception? innerException = null)
        : base(paramName: argument.Name, actualValue: argument.ValueString, message: message)
    {
        ArgumentInfo = argument;
        ExceptionAccess.RefMessageField(this) = message;
        ExceptionAccess.RefInnerExceptionField(this) = innerException;
    }

    public ArgRangeException(
        object? argument,
        string? message = null,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        : base(paramName: argumentName, actualValue: argument, message: message)
    {
        ArgumentInfo = ArgumentInfo.Capture(argument, argumentName);
        ExceptionAccess.RefMessageField(this) = message;
        ExceptionAccess.RefInnerExceptionField(this) = innerException;
    }
}