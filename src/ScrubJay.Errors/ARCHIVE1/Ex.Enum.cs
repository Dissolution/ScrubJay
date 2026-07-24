//namespace ScrubJay.Errors;
//
//public partial class Ex
//{
//    public static InvalidEnumException InvalidEnum<E>(
//        E @enum,
//        string? info = null,
//        Exception? innerException = null,
//        [CallerArgumentExpression(nameof(@enum))]
//        string? enumName = null)
//        where E : struct, Enum
//    {
//        var arg = Argument.Capture(@enum, enumName);
//        var message = TextBuilder.Rent()
//            .Append("Argument ")
//            .Render(arg)
//            .Append(" is not a valid ")
//            .RenderType<E>()
//            .Append(" member")
//            .AppendInfo(info)
//            .ToStringAndDispose();
//        return new InvalidEnumException(arg, message, innerException);
//    }
//}