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
#if NET7_0_OR_GREATER
    IEqualityOperators<LowerBound<T>, LowerBound<T>, bool>,
#endif
    IEquatable<LowerBound<T>>,
    IFormattable, IRenderable
    where T : IComparable<T>
{
    public static implicit operator LowerBound<T>(T value) => Inclusive(value);
    public static implicit operator LowerBound<T>((T Value, BoundCondition Condition) tuple) => new(tuple.Value, tuple.Condition);

    public static bool operator ==(LowerBound<T> left, LowerBound<T> right) => left.Equals(right);
    public static bool operator !=(LowerBound<T> left, LowerBound<T> right) => !left.Equals(right);

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

    public bool Equals(LowerBound<T> other)
    {
        return Any.Equals(Value, other.Value) &&
            Condition == other.Condition;
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

    public void RenderTo(TextBuilder builder)
    {
        if (Condition != BoundCondition.Unbounded)
        {
            if (Condition == BoundCondition.Inclusive)
            {
                builder.Append('[');
            }
            else
            {
                builder.Append('(');
            }
            builder.Render(Value);
        }
        builder.Append("..");
    }
}

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly struct UpperBound<T> :
#if NET7_0_OR_GREATER
    IEqualityOperators<UpperBound<T>, UpperBound<T>, bool>,
#endif
    IEquatable<UpperBound<T>>,
    IFormattable, IRenderable
    where T : IComparable<T>
{
    public static implicit operator UpperBound<T>(T value) => Exclusive(value);
    public static implicit operator UpperBound<T>((T Value, BoundCondition Condition) tuple) => new(tuple.Value, tuple.Condition);

    public static bool operator ==(UpperBound<T> left, UpperBound<T> right) => left.Equals(right);
    public static bool operator !=(UpperBound<T> left, UpperBound<T> right) => !left.Equals(right);

    public static readonly UpperBound<T> Unbounded = new(default!, BoundCondition.Unbounded);
    public static UpperBound<T> Exclusive(T value) => new(value, BoundCondition.Exclusive);
    public static UpperBound<T> Inclusive(T value) => new(value, BoundCondition.Inclusive);

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

    public bool Equals(UpperBound<T> other)
    {
        return Any.Equals(Value, other.Value) &&
            Condition == other.Condition;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is UpperBound<T> upperBound)
            return Equals(upperBound);

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
        text.AppendLiteral("..");
        text.AppendFormatted(Value, format, provider);
        if (Condition == BoundCondition.Inclusive)
            text.AppendFormatted(']');
        else
            text.AppendFormatted(')');
        return text.ToString();
    }

    public override string ToString() => Condition switch
    {
        BoundCondition.Inclusive => $"..{Value}]",
        BoundCondition.Exclusive => $"..{Value})",
        _ => "..",
    };

    public void RenderTo(TextBuilder builder)
    {
        builder.Append("..");
        if (Condition != BoundCondition.Unbounded)
        {
            builder.Render(Value);
            if (Condition == BoundCondition.Inclusive)
            {
                builder.Append(']');
            }
            else
            {
                builder.Append(')');
            }
        }
    }
}

[PublicAPI]
public static class Bounds
{
    public static void RenderTo<T>(TextBuilder builder, LowerBound<T> lower, UpperBound<T> upper)
        where T : IComparable<T>
    {
        builder
            .If(lower.Condition != BoundCondition.Unbounded, ltb => ltb
                .IfAppend(lower.Condition == BoundCondition.Inclusive, '[', '(')
                .Render(lower.Value))
            .Append("..")
            .If(upper.Condition != BoundCondition.Unbounded, utb => utb
                .Render(upper.Value)
                .IfAppend(upper.Condition == BoundCondition.Exclusive, ')', ']'));
    }
}