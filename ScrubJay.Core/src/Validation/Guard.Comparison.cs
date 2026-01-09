namespace ScrubJay.Validation;

partial class Guard
{
    public static T IsBetween<T>(T value, T inclusiveMin, T exclusiveMax,
        [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : IComparable<T>
    {
        int c = Comparer<T>.Default.Compare(value, inclusiveMin);
        if (c < 0)
            throw Ex.ArgRange(value, $"was less than inclusive minimum of {inclusiveMin}", valueName);
        c = Comparer<T>.Default.Compare(value, exclusiveMax);
        if (c >= 0)
            throw Ex.ArgRange(value, $"was greater than or equal to exclusive maximum of {exclusiveMax}", valueName);
        return value;
    }
    
    public static T IsBetween<T>(T value, LowerBound<T> min, UpperBound<T> max,
        [CallerArgumentExpression(nameof(value))] string? valueName = null)
        where T : IComparable<T>
    {
        if (!min.Contains(value))
            throw Ex.ArgRange(value, $"was less than minimum of {min}", valueName);
        if (!max.Contains(value))
            throw Ex.ArgRange(value, $"was greater than maximum of {max}", valueName);
        return value;
    }

    public static T IsLessThan<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
        where T : IComparable<T>
    {
        int c = Comparer<T>.Default.Compare(actual, expected);
        if (c < 0)
            return actual;
        throw Ex.ArgRange(actual, $"was not less than {expected}");
    }
    
    public static T IsLequalTo<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
        where T : IComparable<T>
    {
        int c = Comparer<T>.Default.Compare(actual, expected);
        if (c <= 0)
            return actual;
        throw Ex.ArgRange(actual, $"was not less than or equal to {expected}");
    }
    
    public static T IsGreaterThan<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
        where T : IComparable<T>
    {
        int c = Comparer<T>.Default.Compare(actual, expected);
        if (c > 0)
            return actual;
        throw Ex.ArgRange(actual, $"was not greater than {expected}");
    }
    
    public static T IsGrequalTo<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
        where T : IComparable<T>
    {
        int c = Comparer<T>.Default.Compare(actual, expected);
        if (c >= 0)
            return actual;
        throw Ex.ArgRange(actual, $"was not greater than or equal to {expected}");
    }

    public static T Compares<T>(T actual, T expected, int compare)
        where T : IComparable<T>
    {
        int c = Comparer<T>.Default.Compare(actual, expected);
        if (c == compare)
            return actual;
        throw Ex.ArgRange(actual, $"was did not compare to {expected} as {compare}");
    }
}