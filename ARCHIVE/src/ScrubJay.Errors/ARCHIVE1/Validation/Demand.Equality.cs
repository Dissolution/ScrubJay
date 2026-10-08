//// ReSharper disable MethodOverloadWithOptionalParameter
//
//namespace ScrubJay.Errors.Validation;
//
//public partial class Demand
//{
//    public static void Equal<T>(T? argument, T? expected,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        if (!EqualityComparer<T>.Default.Equals(argument!, expected!))
//            Throw.ArgNotEqual(argument, expected, info, argumentName);
//    }
//
//    public static void Equal<T>(T? argument, T? expected,
//        IEqualityComparer<T> comparer,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        if (!comparer.Equals(argument!, expected!))
//            Throw.ArgNotEqual(argument, expected, info, argumentName);
//    }
//
//#if NET9_0_OR_GREATER
//    public static void Equal<T>(T? argument, T? expected,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null,
//        TypeConstraints.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//    {
//        if (!Any.Equate<T>(in argument, in expected))
//            Throw.ArgNotEqual(argument, expected, info, argumentName);
//    }
//#endif
//
//
//    public static void NotEqual<T>(T? argument, T? expected,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        if (EqualityComparer<T>.Default.Equals(argument!, expected!))
//            Throw.ArgEqual(argument, expected, info, argumentName);
//    }
//
//    public static void NotEqual<T>(T? argument, T? expected,
//        IEqualityComparer<T> comparer,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        if (comparer.Equals(argument!, expected!))
//            Throw.ArgEqual(argument, expected, info, argumentName);
//    }
//
//#if NET9_0_OR_GREATER
//    public static void NotEqual<T>(T? argument, T? expected,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null,
//        TypeConstraints.AllowsRefStruct<T> _ = default)
//        where T : allows ref struct
//    {
//        if (Any.Equate<T>(in argument, in expected))
//            Throw.ArgEqual(argument, expected, info, argumentName);
//    }
//#endif
//}