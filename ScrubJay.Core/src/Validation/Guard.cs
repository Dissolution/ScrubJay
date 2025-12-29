namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Guard
{
    [return: NotNull]
    public static T NotNull<T>([AllowNull, NotNull] T value,
        [CallerArgumentExpression(nameof(value))]
        string? valueName = null)
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
    {
        if (value is not null)
            return value;
        throw Ex.ArgNull(value, valueName: valueName);
    }

    public static int Index(int index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index < (uint)available)
            return index;
        throw Ex.Index(index, available, indexName: indexName);
    }
}