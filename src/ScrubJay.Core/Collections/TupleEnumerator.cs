namespace ScrubJay.Collections;

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public struct TupleEnumerator<T> : IEnumerator<object?>, IEnumerator, IDisposable
    where T : ITuple
{
    private readonly T _tuple;
    private readonly int _count;
    private int _position;

    public readonly object? Current => _tuple[_position];

    public TupleEnumerator(T tuple)
    {
        ArgumentNullException.ThrowIfNull(tuple);
        _tuple = tuple;
        _count = tuple.Length;
        _position = -1;
    }

    readonly void IDisposable.Dispose()
    {
        // do nothing
    }

    public bool MoveNext()
    {
        _position++;
        if (_position < _count)
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _position = -1;
    }
}

[PublicAPI]
[MustDisposeResource(false)]
[StructLayout(LayoutKind.Auto)]
public struct TupleEnumerator<T, V> : IEnumerator<V>, IEnumerator, IDisposable
    where T : ITuple
{
    private readonly T _tuple;
    private readonly int _count;
    private int _position;

    readonly object? IEnumerator.Current => _tuple[_position];
    public readonly V Current => (V)_tuple[_position]!;

    public TupleEnumerator(T tuple)
    {
        ArgumentNullException.ThrowIfNull(tuple);
        var tupleType = tuple.GetType();
        var itemTypes = tupleType.GetGenericArguments();
        if (itemTypes.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(tuple));
        if (itemTypes.Any(type => type != typeof(V)))
            throw new ArgumentOutOfRangeException(nameof(tuple));

        _tuple = tuple;
        _count = tuple.Length;
        _position = -1;
    }

    readonly void IDisposable.Dispose()
    {
        // do nothing
    }

    public bool MoveNext()
    {
        _position++;
        if (_position < _count)
        {
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _position = -1;
    }
}