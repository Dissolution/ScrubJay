namespace ScrubJay.Errors.Validation;

[PublicAPI]
public enum BoundCondition
{
    Unbounded,
    Exclusive,
    Inclusive,
}

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly struct LowerBound<T> :
    IEquatable<LowerBound<T>>, IEquatable<T>,
    IComparable<LowerBound<T>>, IComparable<T>,
#if NET7_0_OR_GREATER
    IEqualityOperators<LowerBound<T>, LowerBound<T>, bool>,
    IEqualityOperators<LowerBound<T>, T, bool>,
    IComparisonOperators<LowerBound<T>, LowerBound<T>, bool>,
    IComparisonOperators<LowerBound<T>, T, bool>,
#endif
    IFormattable
    where T : IComparable<T>
{
    public static implicit operator LowerBound<T>(T value) => Inclusive(value);
    public static implicit operator LowerBound<T>((T Value, BoundCondition Condition) tuple) => new(tuple.Value, tuple.Condition);

    public static bool operator ==(LowerBound<T> left, LowerBound<T> right) => left.Equals(right);
    public static bool operator !=(LowerBound<T> left, LowerBound<T> right) => !left.Equals(right);
    public static bool operator >(LowerBound<T> left, LowerBound<T> right) => left.CompareTo(right) > 0;
    public static bool operator >=(LowerBound<T> left, LowerBound<T> right) => left.CompareTo(right) >= 0;
    public static bool operator <(LowerBound<T> left, LowerBound<T> right) => left.CompareTo(right) < 0;
    public static bool operator <=(LowerBound<T> left, LowerBound<T> right) => left.CompareTo(right) <= 0;

    public static bool operator ==(LowerBound<T> left, T? right) => left.Equals(right);
    public static bool operator !=(LowerBound<T> left, T? right) => !left.Equals(right);
    public static bool operator >(LowerBound<T> left, T right) => left.CompareTo(right) > 0;
    public static bool operator >=(LowerBound<T> left, T right) => left.CompareTo(right) >= 0;
    public static bool operator <(LowerBound<T> left, T right) => left.CompareTo(right) < 0;
    public static bool operator <=(LowerBound<T> left, T right) => left.CompareTo(right) <= 0;

    
    public static readonly LowerBound<T> Unbounded = new(default!, BoundCondition.Unbounded);
    
    public static LowerBound<T> Exclusive(T value) => new(value, BoundCondition.Exclusive);
    
    public static LowerBound<T> Inclusive(T value) => new(value, BoundCondition.Inclusive);


    public readonly T Value;
    
    public readonly BoundCondition Condition;

    private LowerBound(T value, BoundCondition condition)
    {
        Value = value;
        Condition = condition;
    }

    public void Deconstruct(out T value, out BoundCondition condition)
    {
        value = Value;
        condition = Condition;
    }

    public bool Contains(T value)
    {
        if (Condition == BoundCondition.Unbounded)
            return true;

        int c = value.CompareTo(Value);
        return Condition == BoundCondition.Exclusive ? c > 0 : c >= 0;
    }

    public bool Contains(T value, IComparer<T>? comparer)
    {
        if (Condition == BoundCondition.Unbounded)
            return true;

        int c = (comparer ?? Comparer<T>.Default).Compare(value, Value);
        return Condition == BoundCondition.Exclusive ? c > 0 : c >= 0;
    }

    public int CompareTo(LowerBound<T> other)
    {
        switch (Condition)
        {
            case BoundCondition.Unbounded:
            {
                if (other.Condition == BoundCondition.Unbounded)
                    return 0;
                return 1; // infinite > finite
            }
            case BoundCondition.Exclusive:
            {
                if (other.Condition == BoundCondition.Unbounded)
                    return -1; // finite < infinite
                if (other.Condition == BoundCondition.Exclusive)
                    return Any.Compare(Value, other.Value);
                // if (other.Condition == BoundCondition.Inclusive)
                {
                    int c = Any.Compare(Value, other.Value);
                    if (c == 0)
                        return -1; // exclusive < inclusive
                    return c;
                }
            }
            case BoundCondition.Inclusive:
            {
                if (other.Condition == BoundCondition.Unbounded)
                    return -1; // finite < infinite
                if (other.Condition == BoundCondition.Inclusive)
                    return Any.Compare(Value, other.Value);
                // if (other.Condition == BoundCondition.Exclusive)
                {
                    int c = Any.Compare(Value, other.Value);
                    if (c == 0)
                        return 1; // inclusive > exclusive
                    return c;
                }
            }
            default:
                throw Ex.InvalidEnum(Condition);
        }
    }

    public int CompareTo(T? other)
    {
        switch (Condition)
        {
            case BoundCondition.Unbounded:
                return 1; // unbounded > bounded
            case BoundCondition.Exclusive:
            {
                int c = Any.Compare(Value, other);
                if (c == 0)
                    return -1; // actually smaller (not 'in' value)
                return c;
            }
            case BoundCondition.Inclusive:
            {
                return Any.Compare(Value, other);
            }
            default:
                throw Ex.InvalidEnum(Condition);
        }
    }

    public bool Equals(LowerBound<T> other)
    {
        return Any.Equals(Value, other.Value) &&
            Condition == other.Condition;
    }

    public bool Equals(T? value)
    {
        if (Condition != BoundCondition.Inclusive)
            return false;
        return Any.Equals(in Value, in value);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is LowerBound<T> lowerBound)
            return Equals(lowerBound);

        return false;
    }

    public override int GetHashCode()
    {
        return Hasher.HashMany(Value, Condition);
    }

    public string ToString(string? format, IFormatProvider? provider = null)
    {
        if (Condition == BoundCondition.Unbounded)
            return "..";
        using var text = new InterpolatedTextBuilder();
        if (Condition == BoundCondition.Inclusive)
            text.AppendFormatted('[');
        else
            text.AppendFormatted('(');
        text.AppendFormatted(Value, format, provider);
        text.AppendLiteral("..");
        return text.ToString();
    }

    public override string ToString() => Condition switch
    {
        BoundCondition.Inclusive => $"[{Value}..",
        BoundCondition.Exclusive => $"({Value}..",
        _ => "..",
    };
}

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
#pragma warning disable CA1815
public readonly struct UpperBound<T> where T : IComparable<T>
#pragma warning restore CA1815
{
    public static readonly UpperBound<T> Unbounded = new(default!, BoundCondition.Unbounded);
    public static UpperBound<T> Exclusive(T value) => new(value, BoundCondition.Exclusive);
    public static UpperBound<T> Inclusive(T value) => new(value, BoundCondition.Inclusive);

    public static implicit operator UpperBound<T>(T value) => Exclusive(value);

    public readonly T Value;
    public readonly BoundCondition Condition;

    private UpperBound(T value, BoundCondition condition)
    {
        Value = value;
        Condition = condition;
    }

    public void Deconstruct(out T value, out BoundCondition condition)
    {
        value = Value;
        condition = Condition;
    }

    public bool Contains(T value)
    {
        if (Condition == BoundCondition.Unbounded)
            return true;

        int c = value.CompareTo(Value);
        return Condition == BoundCondition.Exclusive ? c < 0 : c <= 0;
    }

    public bool Contains(T value, IComparer<T>? comparer)
    {
        if (Condition == BoundCondition.Unbounded)
            return true;

        int c = (comparer ?? Comparer<T>.Default).Compare(value, Value);
        return Condition == BoundCondition.Exclusive ? c < 0 : c <= 0;
    }

    public override string ToString() => Condition switch
    {
        BoundCondition.Inclusive => $"..{Value}]",
        BoundCondition.Exclusive => $"..{Value})",
        _ => "..",
    };
}

public static class Bounds
{
    public static string GetString<T>(LowerBound<T> lower, UpperBound<T> upper)
        where T : IComparable<T>
    {
        var left = lower.Condition switch
        {
            BoundCondition.Inclusive => $"[{lower.Value}",
            BoundCondition.Exclusive => $"({lower.Value}",
            _ => "",
        };

        var right = upper.Condition switch
        {
            BoundCondition.Inclusive => $"{upper.Value}]",
            BoundCondition.Exclusive => $"{upper.Value})",
            _ => "",
        };

        return $"{left}..{right}";
    }
}