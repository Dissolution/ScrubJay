#if NET9_0_OR_GREATER
namespace ScrubJay.Polyfills.Iteration;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public ref struct RSSingleIteration<T> : IIterable<RSSingleIteration<T>, T>, IIterator<T>
    where T : allows ref struct

{
    public static implicit operator RSSingleIteration<T>(EmptyIteration _) => default;
    public static implicit operator RSSingleIteration<T>(EmptyIteration<T> _) => default;

    private readonly T _value;
    private bool _canYield;

    object? IEnumerator.Current => Box.BoxOrNull(_value);

    public T Current => _value;

    public RSSingleIteration(T value)
    {
        _value = value;
        _canYield = false;
    }

    public bool MoveNext()
    {
        if (_canYield)
        {
            _canYield = false;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        _canYield = true;
    }

    void IDisposable.Dispose()
    {
        // do nothing
    }

    IEnumerator IEnumerable.GetEnumerator() => throw Ex.NotSupported(in this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw Ex.NotSupported(in this);

    public RSSingleIteration<T> GetIterator() => this;
}
#endif