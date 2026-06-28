//namespace ScrubJay.Errors.Exceptions;
//
//[PublicAPI]
//public sealed class ArgRangeException : ArgumentOutOfRangeException
//{
//    public Argument Argument { get; }
//
//    /// <summary>
//    /// Gets the unaltered error message for this Exception.
//    /// </summary>
//    public override string Message => ExceptionFields.RefMessageField(this) ?? "";
//
//    public ArgRangeException(Argument argument, object? argValue, string? message = null, Exception? innerException = null)
//        : base()
//    {
//        Argument = argument;
//        ExceptionFields.RefActualValueField(this) = argValue;
//        ExceptionFields.RefMessageField(this) = message;
//        ExceptionFields.RefInnerExceptionField(this) = innerException;
//    }
//
//    public void RenderTo(TextBuilder builder)
//    {
//        ExceptionRenderer.RenderExceptionTo(this, builder, static (tb, ex) =>
//        {
//            tb.AppendLineIfNotNull(ex.Argument, $"Argument: {ex.Argument:@}")
//                .AppendLineIfNotNull(ex.ActualValue, $"Argument Value: {ex.ActualValue:@}");
//        });
//    }
//
//    public override string ToString() => TextBuilder.Build(RenderTo);
//}