namespace ScrubJay.Memory;

[PublicAPI]
public delegate bool NextItemPredicate<in T>(T item);

[PublicAPI]
public delegate Option<int> NextItemStep<in T>(T item);

[PublicAPI]
public delegate bool NextItemsPredicate<T>(ReadOnlySpan<T> span);

[PublicAPI]
public delegate Option<int> NextItemsStep<T>(ReadOnlySpan<T> span);

[PublicAPI]
public delegate bool PrevNextItemsPredicate<T>(ReadOnlySpan<T> previous, ReadOnlySpan<T> next);

[PublicAPI]
public delegate Option<int> PrevNextItemsStep<T>(ReadOnlySpan<T> previous, ReadOnlySpan<T> next);