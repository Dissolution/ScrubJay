namespace ScrubJay.Functional;

/// <summary>
/// An Option represents an optional value, every Option is either:<br/>
/// <see cref="Some"/> and contains a <typeparamref name="T"/> value
/// or <see cref="None"/>, and does not.
/// </summary>
/// <typeparam name="T">
/// The <see cref="Type"/> of value associated with a <see cref="Some"/> Option
/// </typeparam>
///  <seealso href="https://doc.rust-lang.org/std/option/index.html"/>
/// <remarks>
/// All <c>None</c> values are equal, <c>Some</c> values equate and compare the contained values,
/// <c>None</c> compares less than <c>Some</c>.
/// </remarks>
[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly struct Option<T> : IEnumerable<T>
{
#region Operators
    public static implicit operator bool(Option<T> option) => option._isSome;
    public static explicit operator T(Option<T> option) => option.SomeOrThrow();
    public static explicit operator IMPL.None(Option<T> option) => option.NoneOrThrow();
    public static implicit operator Option<T>(IMPL.None _) => default;
    public static implicit operator Option<T>(T value) => Some(value);
    public static implicit operator Option<T>(IMPL.Ok<T> ok) => Some(ok.Value);
    public static bool operator true(Option<T> option) => option._isSome;
    public static bool operator false(Option<T> option) => !option._isSome;
#endregion
    /// <summary>
    /// Gets <see cref="Option{T}"/>.None, which represents the lack of a value
    /// </summary>
    public static readonly Option<T> None;

    /// <summary>
    /// Get an <see cref="Option{T}"/>.Some containing a <paramref name="value"/>
    /// </summary>
    public static Option<T> Some(T value) => new(value);

    private readonly bool _isSome;
    private readonly T? _value;

    private Option(T value)
    {
        _isSome = true;
        _value = value;
    }

#region None-ness
    public bool IsNone() => !_isSome;

    public bool IsNone(out IMPL.None none)
    {
        none = default;
        return !_isSome;
    }

    public IMPL.None NoneOrThrow(string? exceptionMessage = null)
    {
        if (!_isSome)
            return default;
        exceptionMessage ??= $"The {TypeAlias.For<Option<T>>()} was Some({_value})";
        throw new InvalidOperationException(exceptionMessage);
    }
#endregion
#region Some-ness
    public bool IsSome() => _isSome;

    public bool IsSome([MaybeNullWhen(false)] out T value)
    {
        if (_isSome)
        {
            value = _value!;
            return true;
        }

        value = default;
        return false;
    }

    public T SomeOr(T fallback)
    {
        if (_isSome)
            return _value!;
        return fallback;
    }

    public T SomeOr(Func<T> getFallback)
    {
        if (_isSome)
            return _value!;
        return getFallback();
    }

    public T? SomeOrDefault()
    {
        if (_isSome)
            return _value;
        return default;
    }

    public T SomeOrThrow(string? exceptionMessage = null)
    {
        if (_isSome)
            return _value!;
        exceptionMessage ??= $"The {TypeAlias.For<Option<T>>()} was None";
        throw new InvalidOperationException(exceptionMessage);
    }
#endregion
#region Match
    public void Match(Action<T>? onSome)
    {
        if (_isSome && onSome is not null)
        {
            onSome(_value!);
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

    public void Match(Action<T>? onSome, Action<IMPL.None>? onNone)
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

    public R Match<R>(Func<T, R> some, Func<R> none)
#if NET9_0_OR_GREATER
        where R : allows ref struct
#endif
    {
        if (_isSome)
        {
            return some(_value!);
        }
        else
        {
            return none();
        }
    }

    public R Match<R>(Func<T, R> some, Func<IMPL.None, R> none)
#if NET9_0_OR_GREATER
        where R : allows ref struct
#endif
    {
        if (_isSome)
        {
            return some(_value!);
        }
        else
        {
            return none(default);
        }
    }
#endregion
#region IEnumerable
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    [MustDisposeResource(false)]
    public OptionEnumerator GetEnumerator() => new OptionEnumerator(this);

    [PublicAPI]
    [MustDisposeResource(false)]
    public struct OptionEnumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
        private readonly Option<T> _option;
        private bool _canYield;
        readonly object? IEnumerator.Current => _option.SomeOrThrow();
        public readonly T Current => _option.SomeOrThrow();

        public OptionEnumerator(Option<T> option)
        {
            _option = option;
            _canYield = option._isSome;
        }

        public bool MoveNext()
        {
            if (!_canYield)
            {
                return false;
            }
            else
            {
                _canYield = false;
                return true;
            }
        }

        public void Reset()
        {
            _canYield = _option._isSome;
        }

        readonly void IDisposable.Dispose()
        {
            /* Do Nothing */
        }
    }
#endregion

    public override string ToString()
    {
        if (_isSome)
        {
            return $"Some({_value})";
        }

        return nameof(None);
    }
}