using ScrubJay.Generics;

namespace ScrubJay.Functional;

[PublicAPI]
public static class RefOptionIterable
{
    public static RefOptionIterable<T> None<T>()
        where T : allows ref struct
        => RefOptionIterable<T>.None;

    public static RefOptionIterable<T> Create<T>()
        where T : allows ref struct
        => RefOptionIterable<T>.None;

    public static RefOptionIterable<T> Create<T>(T value)
        where T : allows ref struct
        => new RefOptionIterable<T>(value);

    public static RefOptionIterable<T> Create<T>(Option<T> option)
    {
        if (option._isSome)
        {
            return new(option._value!);
        }
        return default;
    }

//    public static RefValueEnumerator<T> Create<T>(RefOption<T> option)
//        where T : allows ref struct
//    {
//        if (option._isSome)
//        {
//            return new(option._value!);
//        }
//        return default;
//    }

    public static RefOptionIterable<T> Create<T, E>(Result<T, E> result)
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
public ref struct RefOptionIterable<T>
    where T : allows ref struct
{
    public static RefOptionIterable<T> None => default;

    private readonly bool _hasValue;
    private readonly T? _value;
    private bool _canYield;

    public readonly T Current => _hasValue ? _value! : default!;

    public RefOptionIterable()
    {
        _hasValue = false;
        _value = default!;
        _canYield = false;
    }

    public RefOptionIterable(T value)
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

    public readonly RefOptionIterable<T> GetEnumerator() => this;
}