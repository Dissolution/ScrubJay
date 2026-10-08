#if NET9_0_OR_GREATER

using ScrubJay.Errors;
using ScrubJay.Polyfills.Collections;
using ScrubJay.Reflection.Lightweight;
using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
public readonly ref struct RSOption<T>
where T : allows ref struct
{
    public static implicit operator RSOption<T>(T value) => new RSOption<T>(value);
    public static implicit operator RSOption<T>(Impl.None _) => default;
    public static implicit operator bool(RSOption<T> option) => option._some;

    public static bool operator true(RSOption<T> option) => option._some;
    public static bool operator false(RSOption<T> option) => !option._some;
    
    public static RSOption<T> Some(T value) => new RSOption<T>(value);
    public static RSOption<T> None() => default;

    internal readonly bool _some;
    internal readonly T? _value;

    public RSOption(T value)
    {
        _some = true;
        _value = value;
    }

    public RSOption(Impl.None _)
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
        throw new InvalidOperationException($"{TypeName.For<RSOption<T>>()} was not Some");
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
    
    public override string ToString()
    {
        if (_some)
        {
            return $"Some({Any.ToString(_value)})";
        }
        return nameof(None);
    }

#region LINQ + Enumerable
    public RSOption<N> Select<N>(Func<T, N> selector)
    where N : allows ref struct
    {
        if (_some)
        {
            return new RSOption<N>(selector(_value!));
        }
        return default;
    }

    public RSOption<N> Select<N>(Func<T, RSOption<N>> selector)
    {
        if (_some)
        {
            return selector(_value!);
        }

        return default;
    }

//    public RSOption<N> Select<N, E>(Func<T, Result<N, E>> selector)
//    {
//        if (_some)
//        {
//            return selector(_value!);
//        }
//
//        return default;
//    }

    public RSOption<N> SelectMany<K, N>(
        Func<T, RSOption<K>> keySelector,
        Func<T, K, N> newSelector)
    {
        if (_some)
        {
            var keyOption = keySelector(_value!);
            if (keyOption._some)
            {
                var newValue = newSelector(_value!, keyOption._value!);
                return new RSOption<N>(newValue);
            }
        }
        return default;
    }

//    public RSOption<N> SelectMany<K, E, N>(
//        Func<T, Result<K, E>> keySelector,
//        Func<T, K, N> newSelector)
//    {
//        if (_some)
//        {
//            var keyResult = keySelector(_value!);
//            if (keyResult._success)
//            {
//                var newValue = newSelector(_value!, keyResult._value!);
//                return new Option<N>(newValue);
//            }
//        }
//        return default;
//    }
#endregion
}

#endif