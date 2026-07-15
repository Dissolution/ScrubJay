#if NET7_0_OR_GREATER

using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Collections;
using ScrubJay.Reflection.Lightweight;
using ScrubJay.Universal;

namespace ScrubJay.Functional;

[PublicAPI]
public ref struct RefOption<T> :
#if NET9_0_OR_GREATER
    IEquatable<RefOption<T>>,
#endif
    IEnumerable<T>
{
    public static implicit operator RefOption<T>(Impl.None _) => default;
    public static implicit operator bool(RefOption<T> option) => option._some;

    public static bool operator true(RefOption<T> option) => option._some;
    public static bool operator false(RefOption<T> option) => !option._some;

    public static bool operator ==(RefOption<T> left, RefOption<T> right) => left.Equals(right);
    public static bool operator !=(RefOption<T> left, RefOption<T> right) => !left.Equals(right);

    public static RefOption<T> Some(ref T value) => new RefOption<T>(ref value);
    public static RefOption<T> None() => default;

    internal readonly bool _some;
    internal ref T? _value;

    public RefOption(ref T value)
    {
        _some = true;
        _value = ref value!;
    }

    public RefOption(Impl.None _)
    {
        _some = false;
        _value = ref Unsafe.NullRef<T>()!;
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

    public ref T RefSomeOrThrow()
    {
        if (_some)
        {
            return ref _value!;
        }
        throw new InvalidOperationException($"{TypeName.For<Option<T>>()} was not Some");
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

    public void Match(RefAction<T>? onSome, Action<Impl.None>? onNone)
    {
        if (_some)
        {
            onSome?.Invoke(ref _value!);
        }
        else
        {
            onNone?.Invoke(default);
        }
    }

    public void Match(RefAction<T>? onSome, Action? onNone)
    {
        if (_some)
        {
            onSome?.Invoke(ref _value!);
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

    public bool Equals(RefOption<T> other)
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
    [MustDisposeResource(false)]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    [MustDisposeResource(false)]
    public SingleEnumerator<T> GetEnumerator()
    {
        if (_some)
            return new(_value!);
        return default;
    }
#endregion
}

#endif