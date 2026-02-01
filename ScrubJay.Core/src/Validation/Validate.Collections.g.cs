#nullable enable

namespace ScrubJay.Validation;

partial class Validate
{
    public static Result<T[]> IsNotEmpty<T>([AllowNull] T[] array,
        [CallerArgumentExpression(nameof(array))]
        string? arrayName = null)
    {
        if (array is null)
            return Ex.ArgNull(arrayName);
        if (array.Length == 0)
            return Ex.Arg(array, "was empty", arrayName);
        return array;
    }

#if NET9_0_OR_GREATER

    public static RefResult<Span<T>> IsNotEmpty<T>(Span<T> span,
        [CallerArgumentExpression(nameof(span))]
        string? spanName = null)
    {
        if (span.IsEmpty)
            return Ex.Arg(span, "was empty", spanName);
        return span;
    }

    public static RefResult<ReadOnlySpan<T>> IsNotEmpty<T>(ReadOnlySpan<T> span,
        [CallerArgumentExpression(nameof(span))]
        string? spanName = null)
    {
        if (span.IsEmpty)
            return Ex.Arg(span, "was empty", spanName);
        return span;
    }

#endif
}
