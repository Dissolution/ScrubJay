using ScrubJay.Polyfills.Collections;
using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
[AsyncMethodBuilder(typeof(ResultAsyncMethodBuilder<>))] 
public readonly struct Result<T> :
#if NET7_0_OR_GREATER
    IEqualityOperators<Result<T>, Result<T>, bool>,
#endif
    IEquatable<Result<T>>,
    IEnumerable<T>
{

    public static implicit operator Result<T>(T value) => new Result<T>(value);
    public static implicit operator Result<T>(Exception error) => new Result<T>(error);
    public static implicit operator Result<T>(Impl.Ok<T> ok) => new Result<T>(ok._value);
    public static implicit operator Result<T>(Impl.Error<Exception> error) => new Result<T>(error._value);

    public static implicit operator bool(Result<T> result) => result._isOk;
    public static implicit operator Option<T>(Result<T> result) => result.IsOk(out var ok) ? Some(ok) : default;

    public static bool operator true(Result<T> result) => result._isOk;
    public static bool operator false(Result<T> result) => !result._isOk;

    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);
    public static bool operator !=(Result<T> left, Result<T> right) => !left.Equals(right);

    public static Result<T> Ok(T value) => new Result<T>(value);
    public static Result<T> Error(Exception error) => new Result<T>(error);

    internal readonly bool _isOk;
    internal readonly T? _value;
    internal readonly Exception? _error;

    public Result(T value)
    {
        _isOk = true;
        _value = value;
        _error = null;
    }

    public Result(Exception error)
    {
        _isOk = false;
        _value = default(T);
        _error = error;
    }

    public bool IsOk() => _isOk;

    public bool IsOk([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return _isOk;
    }

    public bool IsOk([MaybeNullWhen(false)] out T value, [MaybeNullWhen(true)] out Exception error)
    {
        value = _value;
        error = _error;
        return _isOk;
    }

    public bool IsOkAnd(Func<T, bool> okPredicate)
    {
        return _isOk && okPredicate(_value!);
    }

    public T OkOr(T fallback)
    {
        if (_isOk)
            return _value!;
        return fallback;
    }

    public T OkOr(Func<T> getFallback)
    {
        if (_isOk)
            return _value!;
        return getFallback();
    }

    public T? OkOrDefault()
    {
        if (_isOk)
            return _value!;
        return default(T);
    }

    public T OkOrThrow()
    {
        if (_isOk)
            return _value!;
        throw (_error ?? new InvalidOperationException($"{typeof(Result<T>)} was not Ok"));
    }
    

    public bool IsError() => !_isOk;

    public bool IsError([MaybeNullWhen(false)] out Exception error)
    {
        error = _error;
        return !_isOk;
    }

    public bool IsError([MaybeNullWhen(false)] out Exception error, [MaybeNullWhen(true)] out T value)
    {
        error = _error;
        value = _value;
        return !_isOk;
    }

    public bool IsErrorAnd(Func<Exception, bool> errorPredicate)
    {
        return !_isOk && errorPredicate(_error!);
    }

    public Exception ErrorOr(Exception fallback)
    {
        if (!_isOk)
            return _error!;
        return fallback;
    }

    public Exception ErrorOr(Func<Exception> getFallback)
    {
        if (!_isOk)
            return _error!;
        return getFallback();
    }

    public Exception? ErrorOrDefault()
    {
        if (!_isOk)
            return _error!;
        return null;
    }

    public Exception ErrorOrThrow()
    {
        if (!_isOk)
            return _error!;
        throw new InvalidOperationException($"{typeof(Result<T>)} was not Error");
    }


    public void Match(Action<T>? onOk, Action<Exception>? onError)
    {
        if (_isOk)
        {
            onOk?.Invoke(_value!);
        }
        else
        {
            onError?.Invoke(_error!);
        }
    }

    public R Match<R>(Func<T, R> onOk, Func<Exception, R> onError)
#if NET9_0_OR_GREATER
    where R : allows ref struct
#endif
    {
        if (_isOk)
        {
            return onOk.Invoke(_value!);
        }
        else
        {
            return onError.Invoke(_error!);
        }
    }

    public bool Equals(Result<T> other)
    {
        if (_isOk)
        {
            if (other._isOk)
            {
                return Any.Equals(_value, other._value);
            }
            else
            {
                return false;
            }

        }
        else
        {
            if (other._isOk)
            {
                return false;
            }
            else
            {
                return Any.Equals(_error, other._error);
            }
        }
    }

    public bool Equals(T? other)
    {
        return _isOk && Any.Equals(_value, other);
    }

    public bool Equals(Exception? other)
    {
        return !_isOk && Any.Equals(_error, other);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Result<T> result)
            return Equals(result);
        if (obj is T ok)
            return Equals(ok);
        if (obj is Exception error)
            return Equals(error);
        return false;
    }

    public override int GetHashCode()
    {
        if (_isOk)
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
        if (_isOk)
        {
            return $"Ok({_value})";
        }
        else
        {
            return $"Error({_error})";
        }
    }

#region LINQ + Enumerable
    public Result<N> Select<N>(Func<T, N> selector)
    {
        if (_isOk)
        {
            return new Result<N>(selector(_value!));
        }
        return new Result<N>(_error!);
    }

    public Result<N> Select<N>(Func<T, Result<N>> selector)
    {
        if (_isOk)
        {
            return selector(_value!);
        }

        return new Result<N>(_error!);
    }

    public Result<N> SelectMany<K, N>(
        Func<T, Result<K>> keySelector,
        Func<T, K, N> newSelector)
    {
        if (_isOk)
        {
            var keyResult = keySelector(_value!);
            if (keyResult._isOk)
            {
                var newValue = newSelector(_value!, keyResult._value!);
                return new Result<N>(newValue);
            }

            return new Result<N>(keyResult._error!);
        }
        return new Result<N>(_error!);
    }


    [MustDisposeResource(false)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    public SingleEnumera<T> GetEnumerator()
    {
        if (_isOk)
        {
            return new(_value!);
        }
        return default;
    }
#endregion
}