//#pragma warning disable CS8777
//
//using ScrubJay.Errors.Exceptions;
//
//namespace ScrubJay.Errors.Validation;
//
//[PublicAPI]
//public static partial class Throw;
//
//partial class Throw
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static void IfNull<T>(
//        [AllowNull, NotNull] T? argument,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        if (argument is null)
//        {
//            ArgNullException.Throw(in argument, null, null, argumentName);
//        }
//    }
//
//    public static void IfNullOrEmpty<T>(
//        [AllowNull, NotNull] T[]? array,
//        [CallerArgumentExpression(nameof(array))]
//        string? argumentName = null)
//    {
//        if (array is null || array.Length == 0)
//        {
//            ArgException.Throw(in array, "was null or empty", null, argumentName);
//        }
//    }
//
//    public static void IfNullOrEmpty<T>(
//        [AllowNull, NotNull] ICollection<T>? collection,
//        [CallerArgumentExpression(nameof(collection))]
//        string? argumentName = null)
//    {
//        if (collection is null || collection.Count == 0)
//        {
//            ArgException.Throw(in collection, "was null or empty", null, argumentName);
//        }
//    }
//
//    public static void IfNullOrEmpty(
//        [AllowNull, NotNull] in string? argument,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        if (string.IsNullOrEmpty(argument))
//        {
//            ArgException.Throw(in argument, "was null or empty", null, argumentName);
//        }
//    }
//
//    public static void IfNullOrWhiteSpace(
//        [AllowNull, NotNull] in string? argument,
//        [CallerArgumentExpression(nameof(argument))]
//        string? argumentName = null)
//    {
//        if (string.IsNullOrWhiteSpace(argument))
//        {
//            ArgException.Throw(in argument, "was null, empty, or whitespace", null, argumentName);
//        }
//    }
//
//#region Private Throws
//    [DoesNotReturn]
//    private static void ThrowIsZero<T>(T value, string? argumentName)
//    {
//        throw ArgRangeException.Create(value, "was zero", null, argumentName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowIsNegative<T>(T value, string? argumentName)
//    {
//        throw ArgRangeException.Create(value, "was negative", null, argumentName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowIsNegativeOrZero<T>(T value, string? argumentName)
//    {
//        throw ArgRangeException.Create(value, "was negative or zero", null, argumentName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowIsPositive<T>(T value, string? argumentName)
//    {
//        throw ArgRangeException.Create(value, "was positive", null, argumentName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowIsPositiveOrZero<T>(T value, string? argumentName)
//    {
//        throw ArgRangeException.Create(value, "was positive or zero", null, argumentName);
//    }
//#endregion
//
//#if NET7_0_OR_GREATER
//    public static void IfZero<N>(N number,
//        [CallerArgumentExpression(nameof(number))]
//        string? numberName = null)
//        where N : INumberBase<N>
//    {
//        if (N.IsZero(number))
//            ThrowIsZero<N>(number, numberName);
//    }
//
//    public static void IfNegative<N>(N number,
//        [CallerArgumentExpression(nameof(number))]
//        string? numberName = null)
//        where N : INumberBase<N>
//    {
//        if (N.IsNegative(number))
//            ThrowIsNegative<N>(number, numberName);
//    }
//
//    public static void IfNegativeOrZero<N>(N number,
//        [CallerArgumentExpression(nameof(number))]
//        string? numberName = null)
//        where N : INumberBase<N>
//    {
//        if (N.IsNegative(number) || N.IsZero(number))
//            ThrowIsNegativeOrZero<N>(number, numberName);
//    }
//
//    public static void IfPositive<N>(N number,
//        [CallerArgumentExpression(nameof(number))]
//        string? numberName = null)
//        where N : INumberBase<N>
//    {
//        if (N.IsPositive(number))
//            ThrowIsPositive<N>(number, numberName);
//    }
//
//    public static void IfPositiveOrZero<N>(N number,
//        [CallerArgumentExpression(nameof(number))]
//        string? numberName = null)
//        where N : INumberBase<N>
//    {
//        if (N.IsPositive(number) || N.IsZero(number))
//            ThrowIsPositiveOrZero<N>(number, numberName);
//    }
//
//
//#endif
//}