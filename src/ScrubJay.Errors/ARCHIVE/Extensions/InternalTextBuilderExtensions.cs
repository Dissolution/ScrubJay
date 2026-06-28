//namespace ScrubJay.Errors.Extensions;
//
//internal static class InternalTextBuilderExtensions
//{
//    public static TextBuilder AppendLineIfNotNull<T>(
//        this TextBuilder builder,
//        T? value,
//        [InterpolatedStringHandlerArgument(nameof(builder))]
//        ref InterpolatedTextBuilder interpolatedText)
//    {
//        if (value is not null)
//        {
//            return builder
//                .NewLine()
//                .Append(ref interpolatedText);
//        }
//        return builder;
//    }
//    
//    public static TextBuilder AppendLineIfNotEmpty(
//        this TextBuilder builder,
//        string? str,
//        [InterpolatedStringHandlerArgument(nameof(builder))]
//        ref InterpolatedTextBuilder interpolatedText)
//    {
//        if (!string.IsNullOrEmpty(str))
//        {
//            return builder
//                .NewLine()
//                .Append(ref interpolatedText);
//        }
//        return builder;
//    }
//    
//    public static TextBuilder AppendInfo(
//        this TextBuilder builder,
//        string? info)
//    {
//        return builder.IfNotEmpty(info, static (tb, n) => tb.Append($": {n}"));
//    }
//}