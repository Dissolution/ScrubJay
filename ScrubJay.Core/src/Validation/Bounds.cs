namespace ScrubJay.Validation;

public static class Bounds
{
    public static Bound<T> Unbounded<T>() => new Bound<T>(default!, BoundCondition.Unbounded);
    public static Bound<T> Exclusive<T>(T value) => new Bound<T>(value, BoundCondition.Exclusive);
    public static Bound<T> Inclusive<T>(T value) => new Bound<T>(value, BoundCondition.Inclusive);

    public static string GetString<T>(LowerBound<T> lowerBound, UpperBound<T> upperBound)
    {
        using var builder = new TextBuilder();
        if (lowerBound.Condition > BoundCondition.Unbounded)
        {
            if (lowerBound.Condition == BoundCondition.Exclusive)
            {
                builder.Append('(');
            }
            else
            {
                builder.Append('[');
            }

            builder.Append(lowerBound.Value);
        }

        builder.Append("..");
        if (upperBound.Condition > BoundCondition.Unbounded)
        {
            builder.Append(upperBound.Value);

            if (upperBound.Condition == BoundCondition.Exclusive)
            {
                builder.Append(')');
            }
            else
            {
                builder.Append(']');
            }
        }

        return builder.ToString();
    }
}

public enum BoundCondition
{
    Unbounded,
    Exclusive,
    Inclusive,
}

public readonly struct Bound<T>
{
    public static readonly Bound<T> Unbounded = new Bound<T>(default!, BoundCondition.Unbounded);
    public static Bound<T> Exclusive(T value) => new Bound<T>(value, BoundCondition.Exclusive);
    public static Bound<T> Inclusive(T value) => new Bound<T>(value, BoundCondition.Inclusive);


    public readonly T Value;

    public readonly BoundCondition Condition;

    public Bound(T value, BoundCondition condition)
    {
        Value = value;
        Condition = condition;
    }
}

[StructLayout(LayoutKind.Auto)]
public readonly struct LowerBound<T>
{
    public static implicit operator LowerBound<T>(T value) => new(value);

    public static implicit operator LowerBound<T>((T Value, BoundCondition Condition) tuple)
        => new(tuple.Value, tuple.Condition);

    public static implicit operator LowerBound<T>(Bound<T> bound) => new(bound.Value, bound.Condition);

    public static readonly LowerBound<T> Unbounded = new LowerBound<T>(default!, BoundCondition.Unbounded);
    public static LowerBound<T> Exclusive(T value) => new LowerBound<T>(value, BoundCondition.Exclusive);
    public static LowerBound<T> Inclusive(T value) => new LowerBound<T>(value, BoundCondition.Inclusive);


    public readonly T Value;

    public readonly BoundCondition Condition;

    public LowerBound(T value)
    {
        Value = value;
        Condition = BoundCondition.Inclusive;
    }

    public LowerBound(T value, BoundCondition condition)
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

        int c = Comparer<T>.Default.Compare(Value, value);
        if (Condition == BoundCondition.Exclusive)
            return c < 0;
        return c <= 0;
    }

    public bool Contains(T value, IComparer<T>? comparer)
    {
        if (comparer is null)
            return Contains(value);

        if (Condition == BoundCondition.Unbounded)
            return true;

        int c = comparer.Compare(Value, value);
        if (Condition == BoundCondition.Exclusive)
            return c < 0;
        return c <= 0;
    }

    public override string ToString() => Condition switch
    {
        BoundCondition.Inclusive => $"[{Value}..",
        BoundCondition.Exclusive => $"({Value}..",
        _ => "..",
    };
}

[StructLayout(LayoutKind.Auto)]
public readonly struct UpperBound<T>
{
    public static implicit operator UpperBound<T>(T value) => new(value);

    public static implicit operator UpperBound<T>((T Value, BoundCondition Condition) tuple)
        => new(tuple.Value, tuple.Condition);

    public static implicit operator UpperBound<T>(Bound<T> bound) => new(bound.Value, bound.Condition);


    public static readonly UpperBound<T> Unbounded = new UpperBound<T>(default!, BoundCondition.Unbounded);
    public static UpperBound<T> Exclusive(T value) => new UpperBound<T>(value, BoundCondition.Exclusive);
    public static UpperBound<T> Inclusive(T value) => new UpperBound<T>(value, BoundCondition.Inclusive);


    public readonly T Value;

    public readonly BoundCondition Condition;

    public UpperBound(T value)
    {
        Value = value;
        Condition = BoundCondition.Exclusive;
    }

    public UpperBound(T value, BoundCondition condition)
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

        int c = Comparer<T>.Default.Compare(Value, value);
        if (Condition == BoundCondition.Exclusive)
            return c > 0;
        return c >= 0;
    }

    public bool Contains(T value, IComparer<T>? comparer)
    {
        if (comparer is null)
            return Contains(value);

        if (Condition == BoundCondition.Unbounded)
            return true;

        int c = comparer.Compare(Value, value);
        if (Condition == BoundCondition.Exclusive)
            return c > 0;
        return c >= 0;
    }

    public override string ToString() => Condition switch
    {
        BoundCondition.Inclusive => $"..{Value}]",
        BoundCondition.Exclusive => $"..{Value})",
        _ => "..",
    };
}