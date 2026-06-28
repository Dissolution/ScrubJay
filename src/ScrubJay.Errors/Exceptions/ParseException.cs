//namespace ScrubJay.Errors.Exceptions;
//
//[PublicAPI]
//public sealed class ParseException : FormatException
//{
//    public string? InputString { get; }
//
//    public Type? DestinationType { get; }
//
//    /// <summary>
//    /// Gets the unaltered error message for this Exception.
//    /// </summary>
//    public override string Message => ExceptionFields.RefMessageField(this) ?? "";
//
//    public ParseException(string? inputString, Type? destinationType, string? message = null, Exception? innerException = null)
//        : base()
//    {
//        this.InputString = inputString;
//        this.DestinationType = destinationType;
//        ExceptionFields.RefMessageField(this) = message;
//        ExceptionFields.RefInnerExceptionField(this) = innerException;
//    }
//
//    public void RenderTo(TextBuilder builder)
//    {
//        ExceptionRenderer.RenderExceptionTo(this, builder, static (tb, ex) =>
//        {
//            tb.AppendLineIfNotNull(ex.InputString, $"Input Str: \"{ex.InputString}\"")
//                .AppendLineIfNotNull(ex.DestinationType, $"Dest Type: {ex.DestinationType:@}");
//        });
//    }
//
//    public override string ToString() => TextBuilder.Build(RenderTo);
//}