namespace ScrubJay.Functional;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly struct Result<T, E> : IEnumerable<T>
{
#region Operators
    public static implicit operator bool(Result<T, E> result) => result._isOk;
    public static explicit operator T(Result<T, E> result) => result.OkOrThrow();
    public static explicit operator E(Result<T, E> result) => result.ErrorOrThrow();
    public static implicit operator Result<T, E>(T ok) => Ok(ok);
    public static implicit operator Result<T, E>(E error) => Error(error);
    public static implicit operator Result<T, E>(IMPL.Ok<T> ok) => Ok(ok.Value);
    public static implicit operator Result<T, E>(IMPL.Error<E> error) => Error(error.Value);
    public static bool operator true(Result<T, E> result) => result._isOk;
    public static bool operator false(Result<T, E> result) => !result._isOk;
#endregion

    /// <summary>
    /// Creates a new Ok <see cref="Result{T,E}"/>
    /// </summary>
    public static Result<T, E> Ok(T ok) => new Result<T, E>(true, ok, default);

    /// <summary>
    /// Creates a new Error <see cref="Result{T,E}"/>
    /// </summary>
    public static Result<T, E> Error(E error) => new Result<T, E>(false, default, error);

    // is this Result.Ok?
    // default(Result) implies !_isOk, thus default(Result) == None
    private readonly bool _isOk;

    // if this is Result.Ok, the Ok Value
    private readonly T? _value;

    // if this is Result.Error, the Error Value
    private readonly E? _error;

    private Result(bool isOk, T? value, E? error)
    {
        _isOk = isOk;
        _value = value;
        _error = error;
    }

    public Result(T value)
    {
        _isOk = true;
        _value = value;
        _error = default;
    }

    public Result(E error)
    {
        _isOk = false;
        _value = default;
        _error = error;
    }

#region Ok
    public bool IsOk() => _isOk;

    public bool IsOk([MaybeNullWhen(false)] out T value)
    {
        if (_isOk)
        {
            value = _value!;
            return true;
        }

        value = default;
        return false;
    }

    public bool IsOk([MaybeNullWhen(false)] out T ok, [MaybeNullWhen(true)] out E error)
    {
        ok = _value;
        error = _error;
        return _isOk;
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

    public T OkOrThrow(string? exceptionMessage = null)
    {
        if (_isOk)
            return _value!;
        if (_error is Exception ex)
            throw ex;
        exceptionMessage ??= $"The {TypeAlias.For<Result<T, E>>()} was an Error({_error})";
        throw new InvalidOperationException(exceptionMessage);
    }
#endregion
#region Error
    public bool IsError() => !_isOk;

    public bool IsError([MaybeNullWhen(false)] out E error)
    {
        if (!_isOk)
        {
            error = _error!;
            return true;
        }

        error = default;
        return false;
    }

    public bool IsError([MaybeNullWhen(false)] out E error, [MaybeNullWhen(true)] out T ok)
    {
        error = _error;
        ok = _value;
        return !_isOk;
    }

    public E ErrorOr(E fallback)
    {
        if (!_isOk)
            return _error!;
        return fallback;
    }

    public E ErrorOr(Func<E> getFallback)
    {
        if (_isOk)
            return _error!;
        return getFallback();
    }

    public E? ErrorOrDefault()
    {
        if (!_isOk)
            return _error!;
        return default(E);
    }

    public E ErrorOrThrow(string? exceptionMessage = null)
    {
        if (!_isOk)
            return _error!;
        if (_value is Exception ex)
            throw ex;
        exceptionMessage ??= $"The {TypeAlias.For<Result<T, E>>()} was an Ok({_value})";
        throw new InvalidOperationException(exceptionMessage);
    }
#endregion
#region Match
    public void Match(Action<T>? onOk)
    {
        if (_isOk && onOk is not null)
        {
            onOk(_value!);
        }
    }

    public void Match(Action<T>? onOk, Action<E>? onError)
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

    public R Match<R>(Func<T, R> onOk, Func<E, R> onError)
#if NET9_0_OR_GREATER
        where R : allows ref struct
#endif
    {
        if (_isOk)
        {
            return onOk(_value!);
        }
        else
        {
            return onError(_error!);
        }
    }
#endregion
#region IEnumerable
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    public ResultEnumerator GetEnumerator() => new ResultEnumerator(this);

    [PublicAPI]
    [MustDisposeResource(false)]
    public struct ResultEnumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
        private readonly Result<T, E> _result;
        private bool _canYield;
        object? IEnumerator.Current => _result.OkOrThrow();
        public T Current => _result.OkOrThrow();

        public ResultEnumerator(Result<T, E> result)
        {
            _result = result;
            _canYield = result._isOk;
        }

        void IDisposable.Dispose()
        {
            /* Do Nothing */
        }

        public bool MoveNext()
        {
            if (!_canYield)
            {
                return false;
            }
            _canYield = false;
            return true;
        }

        public void Reset()
        {
            _canYield = _result._isOk;
        }
    }
#endregion

    public Option<T> ToOption()
    {
        if (_isOk)
        {
            return Option.Some(_value!);
        }
        else
        {
            return default;
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
}