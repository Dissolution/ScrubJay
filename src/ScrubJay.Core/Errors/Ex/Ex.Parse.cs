//using ScrubJay.Errors.Exceptions;
//
//namespace ScrubJay.Errors;
//
//public partial class Ex
//{
//    public static ParseException Parse<T>(
//        string? input,
//        string? info = null,
//        Exception? innerException = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//     => ParseException.Create<T>(input, info, innerException);
//
//    public static ParseException Parse<T>(
//        scoped text input,
//        string? info = null,
//        Exception? innerException = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//        => ParseException.Create<T>(input, info, innerException);
//}