namespace ScrubJay.Errors.Validation;

public static partial class Demand
{
    [DoesNotReturn]
    private static void ThrowArgNull<T>(T? _, string? info, string? argumentName)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    {
        throw Ex.ArgNull<T>(_, info, argumentName);
    }

    public static void NotNull<T>(
        [AllowNull, NotNull] T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        where T : class
    {
        if (argument is null)
            ThrowArgNull(argument, info, argumentName);
    }

    public static void NotNull<T>(
        [AllowNull, NotNull] Nullable<T> argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        where T : struct
    {
        if (!argument.HasValue)
            ThrowArgNull(argument, info, argumentName);
    }
}