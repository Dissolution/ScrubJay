namespace ScrubJay.Errors.Exceptions;


/// <summary>
/// An enhanced <see cref="ArgumentNullException"/>.
/// </summary>
[PublicAPI]
public sealed class ArgNullException : ArgumentNullException, IRenderable
{
    public Argument Argument { get; }

    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionFields.RefMessageField(this) ?? "";

    public ArgNullException(Argument argument, string? message = null, Exception? innerException = null)
        : base()
    {
        Argument = argument;
        ExceptionFields.RefParamNameField(this) = Argument.Name;
        ExceptionFields.RefMessageField(this) = message;
        ExceptionFields.RefInnerExceptionField(this) = innerException;
    }

    public void RenderTo(TextBuilder builder)
    {
        ExceptionRenderer.RenderExceptionTo(this, builder, static (tb, ex) =>
        {
            tb.AppendLineIfNotNull(ex.Argument, $"Argument: {ex.Argument:@}");
        });
    }

    public override string ToString() => this.Rendered();
}