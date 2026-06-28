//namespace ScrubJay.Errors;
//
//public partial class Ex
//{
//    internal static ArgException ArgEmpty(Argument argument, string? info)
//    {
//        var message = TextBuilder.Rent()
//            .Append($"Argument {argument:@} was empty")
//            .AppendInfo(info)
//            .ToStringAndDispose();
//        return new ArgException(argument, message);
//    }
//
//    internal static ArgException ArgEmpty<T>(T? argument,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var arg = Argument.Capture<T>(in argument, argumentName);
//        var message = TextBuilder.Rent()
//            .Append($"Argument {arg:@} was empty")
//            .AppendInfo(info)
//            .ToStringAndDispose();
//        return new ArgException(arg, message);
//    }
//}