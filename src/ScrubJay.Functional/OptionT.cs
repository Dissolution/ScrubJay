#pragma warning disable CA1716 // Identifiers should not match keywords

using System.ComponentModel;

namespace ScrubJay.Functional;

[PublicAPI]
[DebuggerDisplay("{ToDebug,nq}")]
[Union]
[StructLayout(LayoutKind.Auto)]
public readonly struct Option<T> :
    IEqualityOperators<Option<T>, Option<T>, bool>,
    IEquatable<Option<T>>,
    IEnumerable<T>
{
    public static implicit operator bool(in Option<T> option) => option._isSome;

    public static bool operator true(Option<T> option) => option._isSome;
    public static bool operator false(Option<T> option) => !option._isSome;

    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
    public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);


    public static Option<T> operator |(Option<T> left, Option<T> right)
    {
        if (left._isSome)
            return left;
        return right;
    }

    public static Option<T> operator ^(Option<T> left, Option<T> right)
    {
        if (left._isSome)
        {
            if (right._isSome)
                return default;
            return left;
        }
        else
        {
            if (right._isSome)
                return right;
            return default;
        }
    }

    public static Option<T> Some(T value) => new Option<T>(value);
    public static Option<T> None() => default;

    internal readonly bool _isSome;
    internal readonly T? _value;

    public Option(T some)
    {
        _isSome = true;
        _value = some;
    }

    public Option(Option.None _)
    {
        _isSome = false;
        _value = default;
    }


#region union

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public bool HasValue => _isSome && _value is not null;

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public object? Value => _isSome ? _value : default(Option.None);

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        if (_isSome && _value is not null)
        {
            value = _value;
            return true;
        }
        value = default;
        return false;
    }

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public bool TryGetValue(out Option.None none)
    {
        none = default;
        return !_isSome;
    }

#endregion

#region Some

    public bool IsSome() => _isSome;

    public bool IsSome([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return _isSome;
    }

    public T SomeOr(T fallback)
    {
        if (_isSome)
            return _value!;
        return fallback!;
    }

    public T SomeOr(Func<T> getFallback)
    {
        if (_isSome)
            return _value!;
        ArgumentNullException.ThrowIfNull(getFallback);
        return getFallback();
    }

    public T SomeOrThrow()
    {
        if (_isSome)
            return _value!;
        throw new InvalidOperationException($"{ToDebug()} is not Some");
    }

#endregion

#region None

    public bool IsNone() => !_isSome;

#endregion

#region Match

    public void Match(Action<T>? onSome)
    {
        if (_isSome)
        {
            onSome?.Invoke(_value!);
        }
    }

    public void Match(Action<T>? onSome, Action? onNone)
    {
        if (_isSome)
        {
            onSome?.Invoke(_value!);
        }
        else
        {
            onNone?.Invoke();
        }
    }

    public void Match(Action<T>? onSome, Action<Option.None>? onNone)
    {
        if (_isSome)
        {
            onSome?.Invoke(_value!);
        }
        else
        {
            onNone?.Invoke(default);
        }
    }

    public R Match<R>(Func<T, R> onSome, Func<R> onNone)
    {
        if (_isSome)
        {
            ArgumentNullException.ThrowIfNull(onSome);
            return onSome.Invoke(_value!);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(onNone);
            return onNone.Invoke();
        }
    }

    public R Match<R>(Func<T, R> onSome, Func<Option.None, R> onNone)
    {
        if (_isSome)
        {
            ArgumentNullException.ThrowIfNull(onSome);
            return onSome.Invoke(_value!);
        }
        else
        {
            ArgumentNullException.ThrowIfNull(onNone);
            return onNone.Invoke(default);
        }
    }

    public Option<R> Match<R>(Func<T, Option<R>>? onSome, Func<Option<R>>? onNone)
    {
        if (_isSome)
        {
            if (onSome is not null)
                return onSome(_value!);
        }
        else
        {
            if (onNone is not null)
                return onNone();
        }

        return default;
    }

    public Option<R> Match<R>(Func<T, Option<R>>? onSome, Func<Option.None, Option<R>>? onNone)
    {
        if (_isSome)
        {
            if (onSome is not null)
                return onSome(_value!);
        }
        else
        {
            if (onNone is not null)
                return onNone(default);
        }

        return default;
    }

#endregion

#region Logic

    public Option<T> Or(Option<T> other)
    {
        if (_isSome)
        {
            return this;
        }
        else
        {
            return other;
        }
    }

    public Option<T> Xor(Option<T> other)
    {
        if (_isSome && !other._isSome)
        {
            return this;
        }

        return other;
    }

    public Option<(T, T)> And(Option<T> other)
    {
        if (_isSome && other._isSome)
        {
            return (_value!, other._value!);
        }
        return default;
    }

#endregion

#region Linq

    // https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/

    public Option<N> OfType<N>()
    {
        if (_isSome && _value is N newValue)
        {
            return newValue;
        }
        return default;
    }

    public Option<T> Where(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (_isSome && predicate(_value!))
        {
            return this;
        }
        return default;
    }

    public Option<N> Select<N>(Func<T, N> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (_isSome)
        {
            return selector(_value!);
        }
        return default;
    }

    public Option<N> SelectMany<N>(Func<T, Option<N>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (_isSome)
        {
            return selector(_value!);
        }
        return default;
    }

    public Option<R> SelectMany<K, R>(
        Func<T, Option<K>> selector,
        Func<T, K, R> projector)
    {
        if (_isSome)
        {
            ArgumentNullException.ThrowIfNull(selector);
            if (selector(_value!).IsSome(out var selection))
            {
                ArgumentNullException.ThrowIfNull(projector);
                return projector(_value!, selection);
            }
        }
        return default;
    }

    public bool All(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _isSome && predicate(_value!);
    }

    public bool Any(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return _isSome && predicate(_value!);
    }

    public bool Contains(T? value)
    {
        return _isSome && EqualityComparer<T>.Default.Equals(_value, value);
    }

    public bool Contains(T? value, IEqualityComparer<T>? comparer)
    {
        return _isSome && (comparer ?? EqualityComparer<T>.Default).Equals(_value, value);
    }

    public IEnumerable<T> AsEnumerable() => ValueEnumerator.Create(this);


    IEnumerator IEnumerable.GetEnumerator() => ValueEnumerator.Create(this);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => ValueEnumerator.Create(this);

    public RefValueEnumerator<T> GetEnumerator() => RefValueEnumerator.Create(this);

#endregion


#region Relation

    public bool Equals(Option<T> other)
    {
        if (_isSome)
        {
            return other._isSome && EqualityComparer<T>.Default.Equals(_value, other._value);
        }
        else
        {
            return !other._isSome;
        }
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Option<T> option && Equals(option);

    public override int GetHashCode()
    {
        if (_isSome)
        {
            return HashCode.Combine(_value);
        }
        else
        {
            return 0; // Option.None's Hash Code
        }
    }

#endregion

#region Display

    internal string ToDebug()
    {
        if (_isSome)
        {
            return $"Option<{typeof(T)}>.Some({_value})";
        }
        else
        {
            return $"Option<{typeof(T)}>.None";
        }
    }

    public override string ToString()
    {
        if (_isSome)
        {
            return $"Ok({_value})";
        }
        else
        {
            return nameof(Option.None);
        }
    }

#endregion
}