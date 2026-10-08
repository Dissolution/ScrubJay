//namespace ScrubJay.Errors.Validation;
//
//public partial class Demand
//{
//    [DoesNotReturn]
//    private static void ThrowIsGreaterOrEqual<T>(T value, T? other, string? paramName)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var arg = Argument.Capture<T>(in value, paramName);
//        var box = Any.BoxOrToString<T>(in value);
//        throw new ArgRangeException(arg, box, R($"Argument {arg:@} was greater than or equal to {other:@}"));
//    }
//
//    public static void LessThan<T>(
//        T value,
//        T? other,
//        [CallerArgumentExpression(nameof(value))]
//        string? valueName = null)
//        where T : IComparable<T>
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (value.CompareTo(other!) >= 0)
//            ThrowIsGreaterOrEqual<T>(value, other, valueName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowIsGreater<T>(T value, T? other, string? paramName)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var arg = Argument.Capture<T>(in value, paramName);
//        var box = Any.BoxOrToString<T>(in value);
//        throw new ArgRangeException(arg, box, R($"Argument {arg:@} was greater than {other:@}"));
//    }
//
//    public static void LessThanOrEqualTo<T>(
//        T value,
//        T? other,
//        [CallerArgumentExpression(nameof(value))]
//        string? valueName = null)
//        where T : IComparable<T>
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (value.CompareTo(other!) > 0)
//            ThrowIsGreater<T>(value, other, valueName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowIsLessOrEqual<T>(T value, T? other, string? paramName)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var arg = Argument.Capture<T>(in value, paramName);
//        var box = Any.BoxOrToString<T>(in value);
//        throw new ArgRangeException(arg, box, R($"Argument {arg:@} was less than or equal to {other:@}"));
//    }
//
//    public static void GreaterThan<T>(
//        T value,
//        T? other,
//        [CallerArgumentExpression(nameof(value))]
//        string? valueName = null)
//        where T : IComparable<T>
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (value.CompareTo(other!) <= 0)
//            ThrowIsLessOrEqual<T>(value, other, valueName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowIsLess<T>(T value, T? other, string? paramName)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        var arg = Argument.Capture<T>(in value, paramName);
//        var box = Any.BoxOrToString<T>(in value);
//        throw new ArgRangeException(arg, box, R($"Argument {arg:@} was less than {other:@}"));
//    }
//
//    public static void GreaterThanOrEqualTo<T>(
//        T value,
//        T? other,
//        [CallerArgumentExpression(nameof(value))]
//        string? valueName = null)
//        where T : IComparable<T>
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (value.CompareTo(other!) < 0)
//            ThrowIsLess<T>(value, other, valueName);
//    }
//
//    [DoesNotReturn]
//    private static void ThrowNotInRange<T>(T value, LowerBound<T> lowerBound, UpperBound<T> upperBound, string? paramName)
//        where T : IComparable<T>
//    {
//        throw Ex.ArgRange(value, lowerBound, upperBound, paramName);
//    }
//
//    public static void InRange<T>(T value, LowerBound<T> lowerBound, UpperBound<T> upperBound,
//        [CallerArgumentExpression(nameof(value))]
//        string? valueName = null)
//        where T : IComparable<T>
//    {
//        if (!lowerBound.Contains(value) || !upperBound.Contains(value))
//            ThrowNotInRange(value, lowerBound, upperBound, valueName);
//    }
//}