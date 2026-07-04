using System.ComponentModel;
using ScrubJay.Polyfills.Collections;
using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
public readonly struct Result<T, E> :
#if NET7_0_OR_GREATER
    IEqualityOperators<Result<T, E>, Result<T, E>, bool>,
#endif
    IEquatable<Result<T, E>>,
    IEnumerable<T>
{

    public static implicit operator Result<T, E>(T value) => new Result<T, E>(value);
    public static implicit operator Result<T, E>(E error) => new Result<T, E>(error);
    public static implicit operator Result<T, E>(Implementations.Ok<T> ok) => new Result<T, E>(ok._value);
    public static implicit operator Result<T, E>(Implementations.Error<E> error) => new Result<T, E>(error._value);

    public static implicit operator bool(Result<T, E> result) => result._success;

    public static bool operator true(Result<T, E> result) => result._success;
    public static bool operator false(Result<T, E> result) => !result._success;

    public static bool operator ==(Result<T, E> left, Result<T, E> right) => left.Equals(right);
    public static bool operator !=(Result<T, E> left, Result<T, E> right) => !left.Equals(right);

    public static Result<T, E> Ok(T value) => new Result<T, E>(value);
    public static Result<T, E> Error(E error) => new Result<T, E>(error);

    internal readonly bool _success;
    internal readonly T? _value;
    internal readonly E? _error;

    public Result(T value)
    {
        _success = true;
        _value = value;
        _error = default(E);
    }

    public Result(E error)
    {
        _success = false;
        _value = default(T);
        _error = error;
    }

    public bool IsOk() => _success;

    public bool IsOk([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return _success;
    }

    public bool IsOk([MaybeNullWhen(false)] out T value, [MaybeNullWhen(true)] out E error)
    {
        value = _value;
        error = _error;
        return _success;
    }

    public bool IsOkAnd(Func<T, bool> okPredicate)
    {
        return _success && okPredicate(_value!);
    }

    public T OkOr(T fallback)
    {
        if (_success)
            return _value!;
        return fallback;
    }

    public T OkOr(Func<T> getFallback)
    {
        if (_success)
            return _value!;
        return getFallback();
    }

    public T? OkOrDefault()
    {
        if (_success)
            return _value!;
        return default(T);
    }

    public T OkOrThrow()
    {
        if (_success)
            return _value!;
        throw new InvalidOperationException($"{typeof(Result<T, E>)} was not Ok");
    }

    public Result<T, Exception> TryGetOk()
    {
        if (_success)
            return new Result<T, Exception>(_value!);
        return new InvalidOperationException($"{typeof(Result<T, E>)} was not Ok");
    }


    public bool IsError() => !_success;

    public bool IsError([MaybeNullWhen(false)] out E error)
    {
        error = _error;
        return !_success;
    }

    public bool IsError([MaybeNullWhen(false)] out E error, [MaybeNullWhen(true)] out T value)
    {
        error = _error;
        value = _value;
        return !_success;
    }

    public bool IsErrorAnd(Func<E, bool> errorPredicate)
    {
        return !_success && errorPredicate(_error!);
    }

    public E ErrorOr(E fallback)
    {
        if (!_success)
            return _error!;
        return fallback;
    }

    public E ErrorOr(Func<E> getFallback)
    {
        if (!_success)
            return _error!;
        return getFallback();
    }

    public E? ErrorOrDefault()
    {
        if (!_success)
            return _error!;
        return default(E);
    }

    public E ErrorOrThrow()
    {
        if (!_success)
            return _error!;
        throw new InvalidOperationException($"{typeof(Result<T, E>)} was not Error");
    }

    public Result<E, Exception> TryGetError()
    {
        if (!_success)
            return new Result<E, Exception>(_error!);
        return new InvalidOperationException($"{typeof(Result<T, E>)} was not Error");
    }



    public void Match(Action<T>? onOk, Action<E>? onError)
    {
        if (_success)
        {
            onOk?.Invoke(_value!);
        }
        else
        {
            onError?.Invoke(_error!);
        }
    }

    public R Match<R>(Func<T, R> onOk, Func<E, R> onError)
#if NET9_0_OR_GREATER
    where R : allows ref struct
#endif
    {
        if (_success)
        {
            return onOk.Invoke(_value!);
        }
        else
        {
            return onError.Invoke(_error!);
        }
    }

    public bool Equals(Result<T, E> other)
    {
        if (_success)
        {
            if (other._success)
            {
                return Relate.Equals(_value, other._value);
            }
            else
            {
                return false;
            }

        }
        else
        {
            if (other._success)
            {
                return false;
            }
            else
            {
                return Relate.Equals(_error, other._error);
            }
        }
    }

    public bool Equals(T? other)
    {
        return _success && Relate.Equals(_value, other);
    }

    public bool Equals(E? other)
    {
        return !_success && Relate.Equals(_error, other);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Result<T, E> result)
            return Equals(result);
        if (obj is T ok)
            return Equals(ok);
        if (obj is E error)
            return Equals(error);
        return false;
    }

    public override int GetHashCode()
    {
        if (_success)
        {
            if (_value is not null)
                return _value.GetHashCode();
            return 1;
        }
        else
        {
            if (_error is not null)
                return _error.GetHashCode();
            return -1;
        }
    }

    public override string ToString()
    {
        if (_success)
        {
            return $"Ok({_value})";
        }
        else
        {
            return $"Error({_error})";
        }
    }

#region LINQ + Enumerable
    public Result<N, E> Select<N>(Func<T, N> selector)
    {
        if (_success)
        {
            return new Result<N, E>(selector(_value!));
        }
        return new Result<N, E>(_error!);
    }

    public Result<N, E> Select<N>(Func<T, Result<N, E>> selector)
    {
        if (_success)
        {
            return selector(_value!);
        }

        return new Result<N, E>(_error!);
    }

    public Result<N, E> SelectMany<K, N>(
        Func<T, Result<K, E>> keySelector,
        Func<T, K, N> newSelector)
    {
        if (_success)
        {
            var keyResult = keySelector(_value!);
            if (keyResult._success)
            {
                var newValue = newSelector(_value!, keyResult._value!);
                return new Result<N, E>(newValue);
            }

            return new Result<N, E>(keyResult._error!);
        }
        return new Result<N, E>(_error!);
    }


    [MustDisposeResource(false)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    public SingleEnumerator<T> GetEnumerator()
    {
        if (_success)
        {
            return new(_value!);
        }
        return default;
    }
#endregion
}