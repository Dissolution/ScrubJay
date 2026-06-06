namespace ScrubJay.Errors.Validation;

public static partial class Demand
{


    public static void NotNull<T>(
        [AllowNull, NotNull] T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        where T : class
    {
        if (argument is null)
            Throw.ArgNull(argument, info, argumentName);
    }

    public static void NotNull<T>(
        [AllowNull, NotNull] Nullable<T> argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        where T : struct
    {
        if (!argument.HasValue)
            Throw.ArgNull(argument, info, argumentName);
    }
}