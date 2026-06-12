namespace ScrubJay.Errors.Validation;

[PublicAPI]
[StackTraceHidden]
public static partial class Throw
{
    [DoesNotReturn]
    public static void Arg<T>(in T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Ex.Arg<T>(in argument, info, argumentName);
    }

    [DoesNotReturn]
    public static void ArgNull<T>(in T? argument, string? info, string? argumentName)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Ex.ArgNull<T>(argument, info, argumentName);
    }

    [DoesNotReturn]
    public static void ArgNotEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Ex.ArgNotEqual<T>(argument, expected, info, argumentName);
    }

    [DoesNotReturn]
    public static void ArgEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Ex.ArgEqual<T>(argument, expected, info, argumentName);
    }
}