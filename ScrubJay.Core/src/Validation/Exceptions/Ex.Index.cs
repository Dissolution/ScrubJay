namespace ScrubJay.Validation;

partial class Ex
{
    public static IndexOutOfRangeException Index(
        int index,
        int available,
        string? info = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        string message = TextBuilder
            .New
            .Append($"An {indexName ?? "Index"} of `{index}` does not fit in [0..{available})")
            .IfNotEmpty(info, static (tb, n) => tb.Append(": ").Write(n))
            .ToStringAndDispose();
        return new IndexOutOfRangeException(message);
    }

    public static IndexOutOfRangeException Index(
        int index,
        int available,
        ref InterpolatedTextBuilder info,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        string message = TextBuilder
            .New
            .Append($"An {indexName ?? "Index"} of `{index}` does not fit in [0..{available})")
            .IfNotEmpty(ref info, static (tb, ref n) => tb.Append(": ").Write(ref n))
            .ToStringAndDispose();
        return new IndexOutOfRangeException(message);
    }

    public static IndexOutOfRangeException Index(
        Index index,
        int available,
        string? info = default,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        string message = TextBuilder
            .New
            .Append($"An {indexName ?? "Index"} of `{index}` does not fit in [0..{available})")
            .IfNotEmpty(info, static (tb, n) => tb.Append(": ").Write(n))
            .ToStringAndDispose();
        return new IndexOutOfRangeException(message);
    }

    public static IndexOutOfRangeException Index(
        Index index,
        int available,
        ref InterpolatedTextBuilder info,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        string message = TextBuilder
            .New
            .Append($"An {indexName ?? "Index"} of `{index}` does not fit in [0..{available})")
            .IfNotEmpty(ref info, static (tb, ref n) => tb.Append(": ").Write(ref n))
            .ToStringAndDispose();
        return new IndexOutOfRangeException(message);
    }

    public static IndexOutOfRangeException Index(
        StackIndex index,
        int available,
        string? info = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        string message = TextBuilder
            .New
            .Append($"An {indexName ?? "Index"} of `{index}` does not fit in [0..{available})")
            .IfNotEmpty(info, static (tb, n) => tb.Append(": ").Write(n))
            .ToStringAndDispose();
        return new IndexOutOfRangeException(message);
    }

    public static IndexOutOfRangeException Index(
        StackIndex index,
        int available,
        ref InterpolatedTextBuilder info,
        Exception? innerException = null,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        string message = TextBuilder
            .New
            .Append($"An {indexName ?? "Index"} of `{index}` does not fit in [0..{available})")
            .IfNotEmpty(ref info, static (tb, ref n) => tb.Append(": ").Write(ref n))
            .ToStringAndDispose();
        return new IndexOutOfRangeException(message);
    }
}