namespace ScrubJay.Polyfills.Iteration;

[PublicAPI]
public interface IIterator : IEnumerator, IDisposable
{

}

[PublicAPI]
public interface IIterator<out TItem> :
    IIterator,
    IEnumerator<TItem>,
    IEnumerator, 
    IDisposable
#if NET9_0_OR_GREATER
    where TItem : allows ref struct
#endif
{

}