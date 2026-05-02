#pragma warning disable CS8764

using ScrubJay.Text.Building;

namespace ScrubJay.Exceptions;

[PublicAPI]
public class ArgException<T> : ArgException, ISJException, IRenderable
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    [SetsRequiredMembers]
    public ArgException(in T? argument, string? message = null, Exception? innerException = null, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        : base(Argument.Capture<T>(in argument, argumentName), message, innerException)
    {

    }
}

/// <summary>
/// An enhanced <see cref="ArgumentException"/>.
/// </summary>
[PublicAPI]
public class ArgException : ArgumentException, ISJException, IRenderable
{
    public required Argument Argument { get; init; }

    public override string? Message
    {
        get
        {
            // ignore the base override, return exactly what is in the message field.
            return ExceptionFields.RefMessageField(this);
        }
    }

    [SetsRequiredMembers]
    public ArgException(Argument argument, string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
        this.Argument = argument;
        ExceptionFields.RefParamNameField(this) = Argument.Name;
    }

    [SetsRequiredMembers]
    public ArgException(object? argument, string? message = null, Exception? innerException = null, [CallerArgumentExpression(nameof(argument))] string? argumentName = null)
        : base(message, innerException)
    {
        this.Argument = Argument.Capture(argument, argumentName);
        ExceptionFields.RefParamNameField(this) = Argument.Name;
    }

    public void RenderTo(TextBuilder builder)
    {
        ExceptionRenderer.RenderExceptionTo(this, builder, static (tb, ex) =>
        {
            tb.AppendLineIfNotNull(ex.Argument, $"Argument: {ex.Argument:@}");
        });
    }

    public override string ToString() => TextBuilder.Build(RenderTo);
}