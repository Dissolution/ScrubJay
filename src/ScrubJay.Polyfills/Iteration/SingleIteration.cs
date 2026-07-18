namespace ScrubJay.Polyfills.Iteration;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public struct SingleIteration<T> : IIterable<SingleIteration<T>, T>, IIterator<T>
{
    public static implicit operator SingleIteration<T>(EmptyIteration _) => default;
    public static implicit operator SingleIteration<T>(EmptyIteration<T> _) => default;
    
    private readonly T _value;
    private bool _canYield;

    object? IEnumerator.Current => _value;
    public T Current => _value;
    
    public SingleIteration(T value)
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

    IEnumerator IEnumerable.GetEnumerator() => this;

    SingleIteration<T> IIterable<SingleIteration<T>>.GetIterator() => this;

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;

    SingleIteration<T> IIterable<SingleIteration<T>, T>.GetIterator() => this;
}