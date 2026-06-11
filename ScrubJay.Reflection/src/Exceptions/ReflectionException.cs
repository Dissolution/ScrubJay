using ScrubJay.Errors;
using ScrubJay.Errors.Exceptions;
using ScrubJay.Errors.Utilities;
using ScrubJay.Text.Building;
#pragma warning disable CA1010

namespace ScrubJay.Reflection.Exceptions;

[PublicAPI]
public class ReflectionException : Exception, ISJException
{
    /// <summary>
    /// Gets the unaltered error message for this Exception.
    /// </summary>
    public override string Message => ExceptionFields.RefMessageField(this) ?? "";

    public ReflectionException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {

    }

    public void RenderTo(TextBuilder builder)
    {
        ExceptionRenderer.RenderExceptionTo(this, builder);
    }

    public override string ToString() => TextBuilder.Build(RenderTo);
}