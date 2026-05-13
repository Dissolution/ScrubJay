namespace ScrubJay.Errors.Exceptions;


/// <summary>
/// An enhanced <see cref="ArgumentNullException"/>.
/// </summary>
[PublicAPI]
public sealed class ArgNullException : ArgumentNullException, IRenderable
{
    public required Argument Argument { get; init; }

    public override string Message
    {
        get
        {
            // ignore the base override, return exactly what is in the message field.
            return ExceptionFields.RefMessageField(this) ?? "";
        }
    }

    [SetsRequiredMembers]
    public ArgNullException(Argument argument, string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
        this.Argument = argument;
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