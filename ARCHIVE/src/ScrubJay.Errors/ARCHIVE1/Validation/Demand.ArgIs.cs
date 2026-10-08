//using ScrubJay.Functional.Utilities;
//
//namespace ScrubJay.Errors.Validation;
//
//public static partial class Demand
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static void ArgIs<T>(
//        in T argument,
//        InPredicate<T> predicate,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        if (!predicate(in argument))
//            Throw.Arg<T>(in argument, info, argumentName);
//    }
//}