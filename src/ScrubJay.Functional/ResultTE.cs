using System.ComponentModel;
using System.Runtime.ExceptionServices;

namespace ScrubJay.Functional;

[PublicAPI]
[DebuggerDisplay("{ToDebug,nq}")]
[Union]
[StructLayout(LayoutKind.Auto)]
public readonly struct Result<T, E> :
    IEqualityOperators<Result<T, E>, Result<T, E>, bool>,
    IEquatable<Result<T, E>>,
    IEnumerable<T>
{
    public static bool operator ==(Result<T, E> left, Result<T, E> right) => left.Equals(right);
    public static bool operator !=(Result<T, E> left, Result<T, E> right) => !left.Equals(right);

    public static Result<T, E> Ok(T value) => new(value);

    public static Result<T, E> Error(E error) => new(error);


    private readonly bool _isOk;
    private readonly T? _value;
    private readonly E? _error;


    public Result(T ok)
    {
        _isOk = true;
        _value = ok;
        _error = default;
    }

    public Result(E error)
    {
        _isOk = false;
        _value = default;
        _error = error;
    }

#region union

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    private bool HasValue => _isOk ? _value is not null : _error is not null;

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public object? Value => _isOk ? _value : _error;

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    private bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        if (_isOk && _value is not null)
        {
            value = _value;
            return true;
        }
        value = default;
        return false;
    }

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    private bool TryGetValue([NotNullWhen(true)] out E? error)
    {
        if (!_isOk && _error is not null)
        {
            error = _error;
            return true;
        }
        error = default;
        return false;
    }

#endregion

#region Ok

    public bool IsOk() => _isOk;

    public bool IsOk([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return _isOk;
    }

    public bool IsOk([MaybeNullWhen(true)] out T value, [MaybeNullWhen(false)] out E error)
    {
        value = _value;
        error = _error;
        return _isOk;
    }

    public Option<T> AsOk()
    {
        if (_isOk)
        {
            return Option<T>.Some(_value!);
        }
        else
        {
            return default;
        }
    }

    public T OkOrThrow()
    {
        if (_isOk)
        {
            return _value!;
        }
        else
        {
            if (_error is Exception exception)
            {
                ExceptionDispatchInfo.Throw(exception);
            }

            throw new InvalidOperationException($"{ToDebug()} is not Ok");
        }
    }

#endregion

#region Error

    public bool IsError() => !_isOk;

    public bool IsError([MaybeNullWhen(false)] out E error)
    {
        error = _error;
        return !_isOk;
    }

    public bool IsError([MaybeNullWhen(false)] out E error, [MaybeNullWhen(true)] out T value)
    {
        value = _value;
        error = _error;
        return !_isOk;
    }

    public Option<E> AsError()
    {
        if (!_isOk)
        {
            return Option<E>.Some(_error!);
        }
        else
        {
            return default;
        }
    }

    public E ErrorOrThrow()
    {
        if (!_isOk)
        {
            return _error!;
        }
        else
        {
            if (_value is Exception exception)
            {
                ExceptionDispatchInfo.Throw(exception);
            }

            throw new InvalidOperationException($"{ToDebug()} is not an Error");
        }
    }

#endregion

#region Linq

    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw new NotImplementedException();

    public ResultRefEnumerator GetEnumerator() => new ResultRefEnumerator(this);

    [StructLayout(LayoutKind.Auto)]
    public ref struct ResultRefEnumerator
    {
        private readonly bool _isOk;
        private readonly T? _value;
        private bool _canYield;

        public readonly T? Current => _isOk ? _value : default;

        public ResultRefEnumerator(Result<T, E> result)
        {
            if (result._isOk)
            {
                _isOk = true;
                _value = result._value;
                _canYield = true;
            }
            else
            {
                _isOk = false;
                _value = default;
                _canYield = false;
            }
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
            if (_isOk)
            {
                _canYield = true;
            }
        }
    }

#endregion

#region Relation

    public bool Equals(Result<T, E> other)
    {
        if (_isOk)
        {
            return other._isOk && EqualityComparer<T>.Default.Equals(_value, other._value);
        }
        else
        {
            return !other._isOk && EqualityComparer<E>.Default.Equals(_error, other._error);
        }
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Result<T, E> result)
            return Equals(result);
        if (obj is T value)
            return _isOk && EqualityComparer<T>.Default.Equals(_value, value);
        if (obj is E error)
            return !_isOk && EqualityComparer<E>.Default.Equals(_error, error);
        return false;
    }

    public override int GetHashCode()
    {
        if (_isOk)
        {
            return HashCode.Combine(_value);
        }
        else
        {
            return HashCode.Combine(_error);
        }
    }

#endregion

#region Display

    internal string ToDebug()
    {
        if (_isOk)
        {
            return $"Result<{typeof(T)}, {typeof(E)}>.Ok({_value})";
        }
        else
        {
            return $"Result<{typeof(T)}, {typeof(E)}>.Error({_error})";
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

#endregion
}