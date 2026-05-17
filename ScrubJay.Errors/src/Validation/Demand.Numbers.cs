#if NET7_0_OR_GREATER

namespace ScrubJay.Errors.Validation;

partial class Demand
{
    [DoesNotReturn]
    private static void ThrowIsZero<T>(T value, string? paramName)
    {
        var arg = Argument.Capture<T>(in value, paramName);
        throw new ArgRangeException(arg, value, $"Argument {arg:@} was zero");
    }

    public static void NotZero<N>(N number,
        [CallerArgumentExpression(nameof(number))]
        string? numberName = null)
        where N : INumberBase<N>
    {
        if (N.IsZero(number))
            ThrowIsZero(number, numberName);

    }

    [DoesNotReturn]
    private static void ThrowIsNegative<T>(T value, string? paramName)
    {
        var arg = Argument.Capture<T>(in value, paramName);
        throw new ArgRangeException(arg, value, $"Argument {arg:@} was negative");
    }

    public static void NotNegative<N>(N number,
        [CallerArgumentExpression(nameof(number))]
        string? numberName = null)
        where N : INumberBase<N>
    {
        if (N.IsNegative(number))
            ThrowIsNegative(number, numberName);
    }

    public static void NotNegativeOrZero<N>(N number,
        [CallerArgumentExpression(nameof(number))]
        string? numberName = null)
        where N : INumberBase<N>
    {
        if (N.IsNegative(number))
            ThrowIsNegative(number, numberName);
        if (N.IsZero(number))
            ThrowIsZero(number, numberName);
    }
}


#endif