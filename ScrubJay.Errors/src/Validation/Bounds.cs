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
public readonly struct LowerBound<T>
    where T : IComparable<T>
{
    public static implicit operator LowerBound<T>(T value) => Inclusive(value);

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
    
    public override string ToString() => Condition switch
    {
        BoundCondition.Inclusive => $"[{Value}..",
        BoundCondition.Exclusive => $"({Value}..",
        _ => "..",
    };
}

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly struct UpperBound<T> where T : IComparable<T>
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