#pragma warning disable CA1716 // Identifiers should not match keywords

using System.ComponentModel;
using ScrubJay.Reflection.Lightweight.Extensions;

namespace ScrubJay.Functional;

[PublicAPI]
[DebuggerDisplay("{ToDebug,nq}")]
[Union]
[StructLayout(LayoutKind.Auto)]
public readonly ref struct RefOption<T> :
    // IEqualityOperators<RefOption<T>, RefOption<T>, bool>,
    IEquatable<RefOption<T>>
    // IEnumerable<T>
    where T : allows ref struct
{
    public static implicit operator bool(in RefOption<T> option) => option._isSome;

    public static bool operator true(RefOption<T> option) => option._isSome;
    public static bool operator false(RefOption<T> option) => !option._isSome;

    public static bool operator ==(RefOption<T> left, RefOption<T> right) => left.Equals(right);
    public static bool operator !=(RefOption<T> left, RefOption<T> right) => !left.Equals(right);


    public static RefOption<T> operator |(RefOption<T> left, RefOption<T> right) => left.Or(right);

    public static RefOption<T> operator ^(RefOption<T> left, RefOption<T> right) => left.Xor(right);

    public static RefOption<T> Some(T value) => new RefOption<T>(value);
    public static RefOption<T> None() => default;

    internal readonly bool _isSome;
    internal readonly T? _value;

    public RefOption(T some)
    {
        _isSome = true;
        _value = some;
    }

    public RefOption(Option.None _)
    {
        _isSome = false;
        _value = default;
    }


    #region union

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public bool HasValue => _isSome && _value is not null && Box.CanBox(_value);

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public object? Value
    {
        get
        {
            if (_isSome)
            {
                if (Box.TryBox(_value, out var boxed))
                    return boxed;
                return null;
            }
            return default(Option.None);
        }
    }

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

    public RefOption<R> Match<R>(Func<T, RefOption<R>>? onSome, Func<RefOption<R>>? onNone)
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

    public RefOption<R> Match<R>(Func<T, RefOption<R>>? onSome, Func<Option.None, RefOption<R>>? onNone)
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

    public RefOption<T> Or(RefOption<T> other)
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

    public RefOption<T> Xor(RefOption<T> other)
    {
        if (_isSome && !other._isSome)
        {
            return this;
        }

        return other;
    }

    #endregion

    #region Linq

    // https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/

    public RefOption<N> OfType<N>()
        where N : allows ref struct
    {
        if (_isSome)
        {
            if (Generic.Is<T, N>(_value!, out var typed))
                return typed;
        }
        return default;
    }

    public RefOption<T> Where(Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (_isSome && predicate(_value!))
        {
            return this;
        }
        return default;
    }

    public RefOption<N> Select<N>(Func<T, N> selector)
        where N : allows ref struct
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (_isSome)
        {
            return selector(_value!);
        }
        return default;
    }

    public RefOption<N> SelectMany<N>(Func<T, RefOption<N>> selector)
        where N : allows ref struct
    {
        ArgumentNullException.ThrowIfNull(selector);
        if (_isSome)
        {
            return selector(_value!);
        }
        return default;
    }

    public RefOption<R> SelectMany<K, R>(
        Func<T, RefOption<K>> selector,
        Func<T, K, R> projector)
        where K : allows ref struct
        where R : allows ref struct
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
        //return _isSome && EqualityComparer<T>.Default.Equals(_value, value);
        throw new NotImplementedException();
    }

    public bool Contains(T? value, IEqualityComparer<T>? comparer)
    {
        //return _isSome && (comparer ?? EqualityComparer<T>.Default).Equals(_value, value);
        throw new NotImplementedException();
    }

    public RefValueEnumerator<T> GetEnumerator() => RefValueEnumerator.Create<T>(this);

    #endregion


    #region Relation

    public bool Equals(RefOption<T> other)
    {
        if (_isSome)
        {
            //return other._isSome && EqualityComparer<T>.Default.Equals(_value, other._value);
            throw new NotImplementedException();
        }
        else
        {
            return !other._isSome;
        }
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => false;

    public override int GetHashCode()
    {
        if (_isSome)
        {
            //return Generic.GetHashCode(in _value);
            throw new NotImplementedException();
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
            return $"RefOption<{typeof(T).Alias}>.Some({Generic.ToString(in _value)})";
        }
        else
        {
            return $"RefOption<{typeof(T).Alias}>.None";
        }
    }

    public override string ToString()
    {
        if (_isSome)
        {
            return $"Ok({Generic.ToString(_value)})";
        }
        else
        {
            return nameof(None);
        }
    }

    #endregion
}