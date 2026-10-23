using System.ComponentModel;

namespace ScrubJay.Functional;

[PublicAPI]
[DebuggerDisplay("{ToDebug,nq}")]
[Union]
[StructLayout(LayoutKind.Auto)]
public readonly struct Option<T> :
    IEqualityOperators<Option<T>, Option<T>, bool>,
    IEquatable<Option<T>>
{
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);
    public static bool operator !=(Option<T> left, Option<T> right) => !left.Equals(right);

    public static Option<T> Some(T value) => new Option<T>(value);
    public static Option<T> None() => default;

    private readonly bool _isSome;
    private readonly T? _value;

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

    public bool IsSome() => _isSome;
    
    public T SomeOr(T fallback)
    {
        if (_isSome)
            return _value!;
        return fallback!;
    }
    
    
#region union

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    private bool HasValue => _isSome && _value is not null;

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    public object? Value => _isSome ? _value : default(Option.None);

    [DebuggerHidden, EditorBrowsable(EditorBrowsableState.Never)]
    private bool TryGetValue([NotNullWhen(true)] out T? value)
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
    private bool TryGetValue(out Option.None none)
    {
        none = default(Option.None);
        return !_isSome;
    }

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

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Option<T> result)
            return Equals(result);
        if (obj is T value)
            return _isSome && EqualityComparer<T>.Default.Equals(_value, value);
        if (obj is Option.None)
            return !_isSome;
        return false;
    }

    public override int GetHashCode()
    {
        if (_isSome)
        {
            return HashCode.Combine(_value);
        }
        else
        {
            return Option.None.HASH_CODE;
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