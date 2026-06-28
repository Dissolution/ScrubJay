using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;
using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Text;

namespace ScrubJay.Errors.Exceptions;

/// <summary>
/// An enhanced <see cref="ArgumentException"/>.
/// </summary>
[PublicAPI]
public sealed class ArgException : ArgumentException, IScrubJayException<ArgException>
{
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

    public static ArgException Create<T>(
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

    public static ArgException Create(
        ArgumentInfo argumentInfo,
        string? info = null,
        Exception? innerException = null)
    {
        using var message = new InterpolatedText();
        message.Write("ArgException: ");
        message.Write(argumentInfo);
        message.Write(" was invalid");
        if (info.IsNotEmpty())
        {
            message.Write(": ");
            message.Write(info);
        }
        return new ArgException(argumentInfo, message.ToString(), innerException);
    }

    public ArgumentInfo ArgumentInfo { get; }

    public override string Message => ExceptionAccess.RefMessageField(this) ?? string.Empty;

    public ArgException(
        ArgumentInfo argument,
        string? message = null,
        Exception? innerException = null)
        : base(message, argument.Name, innerException)
    {
        ArgumentInfo = argument;
    }

    public override string ToString()
    {
        return ExceptionRenderer.RenderException(this,
            static (ref text, exception, indent) =>
            {
                text.NewLine();
                text.Fill(indent * 2, ' ');
                text.Write("ArgumentInfo: ");
                text.Write(exception.ArgumentInfo);
            });
    }
}