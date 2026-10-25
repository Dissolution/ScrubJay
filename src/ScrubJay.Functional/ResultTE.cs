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


    internal readonly bool _isOk;
    internal readonly T? _value;
    internal readonly E? _error;


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

    public T OkOr(T fallback)
    {
        if (_isOk)
            return _value!;
        return fallback!;
    }

    public T OkOr(Func<T> getFallback)
    {
        if (_isOk)
            return _value!;
        return getFallback()!;
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

#region Match

    public void Match(Action<T>? onOk)
    {
        if (_isOk)
        {
            onOk?.Invoke(_value!);
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
    {
        if (_isOk)
        {
            ArgumentNullException.ThrowIfNull(onOk);
            return onOk.Invoke(_value!);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(onError);
            return onError.Invoke(_error!);
        }
    }

#endregion

#region Logic

    // AND, OR, XOR

#endregion

#region Linq

    public bool Contains(T? value)
    {
        return _isOk && EqualityComparer<T>.Default.Equals(_value, value);
    }

    public bool Contains(T? value, IEqualityComparer<T>? comparer)
    {
        return _isOk && (comparer ?? EqualityComparer<T>.Default).Equals(_value, value);
    }

    public bool Any() => _isOk;

    public bool Any(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _isOk && predicate(_value!);
    }


    public Result<N, E> Select<N>(Func<T, N> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (_isOk)
        {
            return selector(_value!);
        }
        else
        {
            return _error!;
        }
    }

    public Result<N, E> Select<N>(Func<T, Result<N, E>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (_isOk)
        {
            return selector(_value!);
        }

        return _error!;
    }

    public Result<N, E> Select<N>(Func<T, Option<N>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (_isOk && selector(_value!).IsSome(out var selected))
        {
            return selected;
        }

        return default;
    }


    public Result<N, E> SelectMany<N>(Func<T, Result<N, E>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        if (_isOk)
        {
            return selector(_value!);
        }
        else
        {
            return _error!;
        }
    }

    public Result<N, E> SelectMany<N>(Func<T, Option<N>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (_isOk && selector(_value!).IsSome(out var selected))
        {
            return selected;
        }
        else
        {
            return _error!;
        }
    }

    public Result<N, E> SelectMany<K, N>(Func<T, Result<K, E>> selector, Func<T, K, N> projector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(projector);
        if (_isOk)
        {
            var selected = selector(_value!);
            if (selected._isOk)
            {
                return projector(_value!, selected._value!);
            }
            return selected._error!;
        }
        return _error!;
    }

    public Result<N, E> SelectMany<K, N>(Func<T, Option<K>> selector, Func<T, K, N> projector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        ArgumentNullException.ThrowIfNull(projector);
        if (_isOk && selector(_value!).IsSome(out var selected))
        {
            return projector(_value!, selected);
        }
        return _error!;
    }

//
//    public Option<T> Where(Func<T, bool> predicate)
//    {
//        ArgumentNullException.ThrowIfNull(predicate);
//        if (_isOk && predicate(_value!))
//        {
//            return _value!;
//        }
//        return default;
//    }

    public IEnumerable<T> ToEnumerable() => this;

    IEnumerator IEnumerable.GetEnumerator() => ValueEnumerator.Create(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ValueEnumerator.Create(this);

    public RefValueEnumerator<T> GetEnumerator() => RefValueEnumerator.Create(this);

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