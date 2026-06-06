namespace ScrubJay.Errors.Validation;

partial class Demand
{
    [DoesNotReturn]
    private static void ThrowArgEmpty<T>(T? argument, string? info, string? argumentName)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        throw Ex.ArgEmpty<T>(argument, info, argumentName);
    }

    [DoesNotReturn]
    private static void ThrowArgEmpty<T>(scoped ReadOnlySpan<T> argument, string? info, string? argumentName)
    {
        throw Ex.ArgEmpty(Argument.Capture(argument, argumentName), info);
    }

    public static void NotEmpty<T>(
        [AllowNull, NotNull] T[]? array,
        string? info = null,
        [CallerArgumentExpression(nameof(array))]
        string? argumentName = null)
    {
        if (array is null)
            Throw.ArgNull(array, info, argumentName);
        if (array.Length == 0)
            ThrowArgEmpty(array, info, argumentName);
    }

    public static void NotEmpty<T>(
        scoped ReadOnlySpan<T> span,
        string? info = null,
        [CallerArgumentExpression(nameof(span))]
        string? argumentName = null)
    {
        if (span.IsEmpty)
            ThrowArgEmpty(span, info, argumentName);
    }

    public static void NotEmpty<C, T>(
        C? collection,
        string? info = null,
        [CallerArgumentExpression(nameof(collection))]
        string? argumentName = null)
        where C : ICollection<T>
    {
        if (collection is null)
            Throw.ArgNull(collection, info, argumentName);
        if (collection.Count == 0)
            ThrowArgEmpty(collection, info, argumentName);
    }

}