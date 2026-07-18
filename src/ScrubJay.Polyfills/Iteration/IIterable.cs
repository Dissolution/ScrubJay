namespace ScrubJay.Polyfills.Iteration;

[PublicAPI]
public interface IIterable<out TIterator> : IEnumerable
    where TIterator : IIterator
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
{
    TIterator GetIterator();
}

[PublicAPI]
public interface IIterable<out TIterator, out TItem> : IIterable<TIterator>, IEnumerable<TItem>, IEnumerable
    where TIterator : IIterator<TItem>
#if NET9_0_OR_GREATER
    , allows ref struct
    where TItem : allows ref struct
#endif
{
    new TIterator GetIterator();
}

