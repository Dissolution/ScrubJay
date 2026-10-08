namespace ScrubJay.Polyfills.Collections;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public struct SingleEnumera<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
{
    public static implicit operator SingleEnumera<T>(EmptyEnumera _) => default;
    public static implicit operator SingleEnumera<T>(EmptyEnumera<T> _) => default;

    private readonly T _value;
    private bool _canYield;

    readonly object? IEnumerator.Current => _value;
    public readonly T Current => _value;

    public SingleEnumera(T value)
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

    public SingleEnumera<T> GetEnumerator() => this;
}