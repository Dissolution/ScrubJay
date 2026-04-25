#pragma warning disable CS8764

using ScrubJay.Text.Building;
using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

[PublicAPI]
public class ArgException<T> : ArgException, ISJException
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public ArgException(in T? argument, string? message = null, Exception? innerException = null, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        : base(Argument.Capture<T>(in argument, argumentName), message, innerException)
    {

    }
}

/// <summary>
/// An enhanced <see cref="ArgumentException"/>.
/// </summary>
[PublicAPI]
public class ArgException : ArgumentException, ISJException
{
    public Argument Argument { get; }

    public override string? Message
    {
        get
        {
            // ignore the base override, return exactly what is in the message field.
            return ExceptionFields.RefMessageField(this);
        }
    }

    public ArgException(Argument argument, string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
        this.Argument = argument;
        ExceptionFields.RefParamNameField(this) = Argument.Name;
    }

    public ArgException(object? argument, string? message = null, Exception? innerException = null, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        : base(message, innerException)
    {
        this.Argument = Argument.Capture(argument, argumentName);
        ExceptionFields.RefParamNameField(this) = Argument.Name;
    }

    public override string ToString()
    {
        using var builder = TextBuilder.Rent();

        builder.Append("ArgumentException:")
            .Indent("  ")
            .NewLine()
            .Append("Argument: ")
            .IfNotNull(Argument, static (tb, arg) => arg.WriteTo(tb), TB.Write("null"))
            .IfNotEmpty(Message, static (tb, msg) => tb.NewLine().Append("Message: ").Append(msg));
     
        this.WriteDebugInformationTo(ref text, 1);
        
        this.WriteOptionalPropertiesTo(ref text, 1);

        return text.ToStringAndDispose();
    }
}