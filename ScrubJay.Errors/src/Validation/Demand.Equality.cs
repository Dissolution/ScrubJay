// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Errors.Validation;

partial class Demand
{
    [DoesNotReturn]
    private static void ThrowArgNotEqual<T>(T? argument, T? expected, string? info, string? argumentName)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Ex.ArgNotEqual<T>(argument, expected, info, argumentName);
    }

    public static void Equal<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(argument!, expected!))
            ThrowArgNotEqual(argument, expected, info, argumentName);
    }

    public static void Equal<T>(T? argument, T? expected,
        IEqualityComparer<T> comparer,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        if (!comparer.Equals(argument!, expected!))
            ThrowArgNotEqual(argument, expected, info, argumentName);
    }

#if NET9_0_OR_GREATER
    public static void Equal<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (!Any.Equals<T>(in argument, in expected))
            ThrowArgNotEqual(argument, expected, info, argumentName);
    }
#endif


    [DoesNotReturn]
    private static void ThrowArgEqual<T>(T? argument, T? expected, string? info, string? argumentName)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Ex.ArgEqual<T>(argument, expected, info, argumentName);
    }

    public static void NotEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        if (EqualityComparer<T>.Default.Equals(argument!, expected!))
            ThrowArgEqual(argument, expected, info, argumentName);
    }

    public static void NotEqual<T>(T? argument, T? expected,
        IEqualityComparer<T> comparer,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        if (comparer.Equals(argument!, expected!))
            ThrowArgEqual(argument, expected, info, argumentName);
    }

#if NET9_0_OR_GREATER
    public static void NotEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (Any.Equals<T>(in argument, in expected))
            ThrowArgEqual(argument, expected, info, argumentName);
    }
#endif
}