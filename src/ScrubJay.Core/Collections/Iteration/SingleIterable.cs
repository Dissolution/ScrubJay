namespace ScrubJay.Collections.Iteration;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public struct SingleIterable<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
{
    public static implicit operator SingleIterable<T>(EmptyIterable _) => default;
    public static implicit operator SingleIterable<T>(EmptyIterable<T> _) => default;

    private readonly T _value;
    private bool _canYield;

    readonly object? IEnumerator.Current => _value;
    public readonly T Current => _value;

    public SingleIterable(T value)
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

    readonly void IDisposable.Dispose()
    {
        // do nothing
    }

    IEnumerator IEnumerable.GetEnumerator() => this;

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this;

    public SingleIterable<T> GetEnumerator() => this;
}