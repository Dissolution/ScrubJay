namespace ScrubJay.Functional;

[PublicAPI]
public static class RefValueEnumerator
{
    public static RefValueEnumerator<T> None<T>()
        where T : allows ref struct
        => RefValueEnumerator<T>.None;

    public static RefValueEnumerator<T> Create<T>()
        where T : allows ref struct
        => RefValueEnumerator<T>.None;

    public static RefValueEnumerator<T> Create<T>(T value)
        where T : allows ref struct
        => RefValueEnumerator<T>.Create(value);

    public static RefValueEnumerator<T> Create<T>(Option<T> option)
    {
        if (option._isSome)
        {
            return new(option._value!);
        }
        return default;
    }

    public static RefValueEnumerator<T> Create<T, E>(Result<T, E> result)
    {
        if (result._isOk)
        {
            return new(result._value!);
        }
        return default;
    }
}

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public ref struct RefValueEnumerator<T>
    where T : allows ref struct
{
    public static RefValueEnumerator<T> None
    {
        get => default;
    }

    public static RefValueEnumerator<T> Create() => default;

    public static RefValueEnumerator<T> Create(T value) => new(value);


    private readonly bool _hasValue;
    private readonly T? _value;
    private bool _canYield;

    public readonly T Current => _hasValue ? _value! : default!;

    public RefValueEnumerator()
    {
        _hasValue = false;
        _value = default!;
        _canYield = false;
    }

    public RefValueEnumerator(T value)
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

    public RefValueEnumerator<T> GetEnumerator() => this;
}