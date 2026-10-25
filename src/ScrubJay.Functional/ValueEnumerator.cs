namespace ScrubJay.Functional;

[PublicAPI]
public static class ValueEnumerator
{
    public static ValueEnumerator<T> None<T>() => ValueEnumerator<T>.None;

    [MustDisposeResource(false)]
    public static ValueEnumerator<T> Create<T>() => ValueEnumerator<T>.None;

    [MustDisposeResource(false)]
    public static ValueEnumerator<T> Create<T>(T value) => ValueEnumerator<T>.Create(value);

    [MustDisposeResource(false)]
    public static ValueEnumerator<T> Create<T>(Option<T> option)
    {
        if (option._isSome)
        {
            return new(option._value!);
        }
        return ValueEnumerator<T>.None;
    }

    [MustDisposeResource(false)]
    public static ValueEnumerator<T> Create<T, E>(Result<T, E> result)
    {
        if (result._isOk)
        {
            return new(result._value!);
        }
        return ValueEnumerator<T>.None;
    }
}

[PublicAPI]
[MustDisposeResource(false)]
public sealed class ValueEnumerator<T> :
    IEnumerable<T>, IEnumerable,
    IEnumerator<T>, IEnumerator, IDisposable
{
    public static ValueEnumerator<T> None
    {
        [MustDisposeResource(false)]
        get;
    } = new();

    [MustDisposeResource(false)]
    public static ValueEnumerator<T> Create() => None;

    [MustDisposeResource(false)]
    public static ValueEnumerator<T> Create(T value) => new(value);


    private readonly bool _hasValue;
    private readonly T? _value;
    private bool _canYield;

    object? IEnumerator.Current => _value;
    public T Current => _value!;

    public ValueEnumerator()
    {
        _hasValue = false;
        _value = default!;
        _canYield = false;
    }

    public ValueEnumerator(T value)
    {
        _hasValue = true;
        _value = value;
        _canYield = true;
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
        _canYield = _hasValue;
    }

    void IDisposable.Dispose()
    {
        // do nothing
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    public ValueEnumerator<T> GetEnumerator() => this;
}