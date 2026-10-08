//namespace ScrubJay.Errors.Validation;
//
//public static partial class Validate
//{
//    public static Result<T> NotNull<T>(
//        T? argument,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//        where T : class
//    {
//        if (argument is null)
//            return Ex.ArgNull(argument, info, argumentName);
//        return argument;
//    }
//
//    public static Result<T> NotNull<T>(
//        Nullable<T> argument,
//        string? info = null,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//        where T : struct
//    {
//        if (argument is null)
//            return Ex.ArgNull(argument, info, argumentName);
//        return argument.GetValueOrDefault();
//    }
//}