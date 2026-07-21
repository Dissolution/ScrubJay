using ScrubJay.Polyfills.Collections;
using ScrubJay.Reflection.Lightweight;
using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
public readonly struct Option<T> :
#if NET7_0_OR_GREATER
    IEqualityOperators<Option<T>, Option<T>, bool>,
#endif
    IEquatable<Option<T>>,
    IEnumerable<T>
{
    public static implicit operator Option<T>(T value) => new Option<T>(value);
    public static implicit operator Option<T>(Impl.None _) => default;
    public static implicit operator bool(Option<T> option) => option._some;

    public static bool operator true(Option<T> option) => option._some;
    public static bool operator false(Option<T> option) => !option._some;

    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
    public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);

    public static Option<T> Some(T value) => new Option<T>(value);
    public static Option<T> None() => default;

    internal readonly bool _some;
    internal readonly T? _value;

    public Option(T value)
    {
        _some = true;
        _value = value;
    }

    public Option(Impl.None _)
    {
        _some = false;
        _value = default(T);
    }

    public bool IsSome() => _some;

    public bool IsSome([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return _some;
    }

    public bool IsSomeAnd(Func<T, bool> okPredicate)
    {
        return _some && okPredicate(_value!);
    }

    public T SomeOr(T fallback)
    {
        if (_some)
            return _value!;
        return fallback;
    }

    public T SomeOr(Func<T> getFallback)
    {
        if (_some)
            return _value!;
        return getFallback();
    }

    public T? SomeOrDefault()
    {
        if (_some)
            return _value!;
        return default(T);
    }

    public T SomeOrThrow()
    {
        if (_some)
            return _value!;
        throw new InvalidOperationException($"{TypeName.For<Option<T>>()} was not Some");
    }

    public Result<T, Exception> TryGetSome()
    {
        if (_some)
            return new Result<T, Exception>(_value!);
        return new InvalidOperationException($"{TypeName.For<Option<T>>()} was not Some");
    }


    public bool IsNone() => !_some;

    public bool IsNoneAnd(Func<bool> errorPredicate)
    {
        return !_some && errorPredicate();
    }

    public bool IsNoneAnd(Func<Impl.None, bool> errorPredicate)
    {
        return !_some && errorPredicate(default);
    }


    public void Match(Action<T>? onSome, Action? onNone)
    {
        if (_some)
        {
            onSome?.Invoke(_value!);
        }
        else
        {
            onNone?.Invoke();
        }
    }

    public void Match(Action<T>? onSome, Action<Impl.None>? onNone)
    {
        if (_some)
        {
            onSome?.Invoke(_value!);
        }
        else
        {
            onNone?.Invoke(default);
        }
    }

    public R Match<R>(Func<T, R> onSome, Func<R> onNone)
#if NET9_0_OR_GREATER
        where R : allows ref struct
#endif
    {
        if (_some)
        {
            return onSome.Invoke(_value!);
        }
        else
        {
            return onNone.Invoke();
        }
    }

    public R Match<R>(Func<T, R> onSome, Func<Impl.None, R> onNone)
#if NET9_0_OR_GREATER
        where R : allows ref struct
#endif
    {
        if (_some)
        {
            return onSome.Invoke(_value!);
        }
        else
        {
            return onNone.Invoke(default);
        }
    }


    public bool Equals(Option<T> other)
    {
        if (_some)
        {
            if (other._some)
                return Any.Equals(_value, other._value);

            return false;
        }
        return !other._some;
    }

    public bool Equals(T? other)
    {
        return _some && Any.Equals(_value, other);
    }

    public bool Equals(Impl.None _)
    {
        return !_some;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Option<T> option)
            return Equals(option);
        if (obj is T some)
            return Equals(some);
        if (obj is Impl.None none)
            return Equals(none);
        return false;
    }

    public override int GetHashCode()
    {
        if (_some)
        {
            if (_value is not null)
                return _value.GetHashCode();
            return 1;
        }
        return 0;
    }

    public override string ToString()
    {
        if (_some)
        {
            return $"Some({_value})";
        }
        return nameof(None);
    }

#region LINQ + Enumerable
    public Option<N> Select<N>(Func<T, N> selector)
    {
        if (_some)
        {
            return new Option<N>(selector(_value!));
        }
        return default;
    }

    public Option<N> Select<N>(Func<T, Option<N>> selector)
    {
        if (_some)
        {
            return selector(_value!);
        }

        return default;
    }
    
    public Option<N> Select<N,E>(Func<T, Result<N,E>> selector)
    {
        if (_some)
        {
            return selector(_value!);
        }

        return default;
    }

    public Option<N> SelectMany<K, N>(
        Func<T, Option<K>> keySelector,
        Func<T, K, N> newSelector)
    {
        if (_some)
        {
            var keyOption = keySelector(_value!);
            if (keyOption._some)
            {
                var newValue = newSelector(_value!, keyOption._value!);
                return new Option<N>(newValue);
            }
        }
        return default;
    }
    
    public Option<N> SelectMany<K, E, N>(
        Func<T, Result<K, E>> keySelector,
        Func<T, K, N> newSelector)
    {
        if (_some)
        {
            var keyResult = keySelector(_value!);
            if (keyResult._isOk)
            {
                var newValue = newSelector(_value!, keyResult._value!);
                return new Option<N>(newValue);
            }
        }
        return default;
    }


    [MustDisposeResource(false)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    public SingleEnumera<T> GetEnumerator()
    {
        if (_some)
            return new(_value!);
        return default;
    }
#endregion
}