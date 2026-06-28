//using System.Reflection;
//
//namespace ScrubJay.Errors;
//
//[PublicAPI]
//public static class ExceptionRenderer
//{
//    internal static void RenderCodePropertiesTo(Exception exception, TextBuilder builder)
//    {
//        string? source = exception.Source;
//        MethodBase? targetSite = exception.TargetSite;
//        string? stackTrace = exception.StackTrace;
//
//        if (source is not null || targetSite is not null)
//        {
//            builder.NewLine()
//                .Append("Target: ");
//            if (source is not null)
//            {
//                builder.Append(source);
//                if (targetSite is not null)
//                    builder.Append('.');
//            }
//            if (targetSite is not null)
//            {
//                builder.Render(targetSite);
//            }
//        }
//
//        if (stackTrace is not null)
//        {
//            builder.NewLine()
//                .Append($"StackTrace: {stackTrace}");
//        }
//    }
//
//    internal static void RenderOptionalPropertiesTo(Exception exception, TextBuilder builder)
//    {

//    }
//
//    internal static void RenderExceptionTo<E>(E exception, TextBuilder builder,
//        Action<TextBuilder, E>? renderAdditionalProperties)
//        where E : Exception
//    {
//        builder
//            .RenderTypeOf(exception).Append(':')
//            .Indent()
//            .AppendLineIfNotEmpty(exception.Message, $"Message: {exception.Message}")
//            .Invoke(exception, renderAdditionalProperties)
//            .Invoke(exception, RenderCodePropertiesTo)
//            .Invoke(exception, RenderOptionalPropertiesTo)
//            .Outdent();
//    }
//
//    [RenderToMethod]
//    public static void RenderExceptionTo<E>(E exception, TextBuilder builder)
//        where E : Exception
//    {
//        RenderExceptionTo<E>(exception, builder, null);
//    }
//}