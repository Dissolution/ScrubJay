namespace ScrubJay.Validation;

partial class Guard
{
    public static T[] IsNotEmpty<T>([AllowNull, NotNull] T[] array,
        [CallerArgumentExpression(nameof(array))]
        string? arrayName = null)
    {
        if (array is null)
            throw Ex.ArgNull<T[]>(arrayName);
        if (array.Length == 0)
            throw Ex.Arg(array, "was empty", arrayName);
        return array;
    }

//#if NET9_0_OR_GREATER

    public static Span<T> IsNotEmpty<T>(Span<T> span,
        [CallerArgumentExpression(nameof(span))]
        string? spanName = null)
    {
        if (span.IsEmpty)
            throw Ex.Arg(span, "was empty", spanName);
        return span;
    }

    public static ReadOnlySpan<T> IsNotEmpty<T>(ReadOnlySpan<T> span,
        [CallerArgumentExpression(nameof(span))]
        string? spanName = null)
    {
        if (span.IsEmpty)
            throw Ex.Arg<T>(span, "was empty", spanName);
        return span;
    }

//#endif    
}