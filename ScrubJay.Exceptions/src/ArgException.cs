#pragma warning disable CS8764

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
        var text = new InterpolatedText(stackalloc char[512]);
        
        text.AppendLiteral("ArgumentException:");
       
        text.AppendLiteral(Environment.NewLine); 
        text.AppendLiteral("  Argument: ");
        if (Argument is not null)
        {
            Argument.WriteTo(ref text);
        }
        else
        {
            text.AppendLiteral("null");
        }

        var message = Message;
        if (!string.IsNullOrEmpty(message))
        {
            text.AppendLiteral(Environment.NewLine);
            text.AppendLiteral("  Message: ");
            text.AppendLiteral(message!);
        }

        this.WriteOptionalPropertiesTo(ref text);

        return text.ToStringAndDispose();
    }
}