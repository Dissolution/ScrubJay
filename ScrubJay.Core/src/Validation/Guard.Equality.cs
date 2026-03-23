namespace ScrubJay.Validation;

partial class Guard
{
    public static T IsEqual<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (EqualityComparer<T>.Default.Equals(actual, expected))
            return actual;
        throw Ex.Arg(actual, $"was not equal to {expected}", actualName);
    }
    
    public static T IsNotEqual<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            return actual;
        throw Ex.Arg(actual, $"was equal to {expected}", actualName);
    }
    
    public static T IsEqual<T>(T actual, T expected, IEqualityComparer<T>? comparer,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (comparer is null)
            return IsEqual<T>(actual, expected, actualName);
        
        if (comparer.Equals(actual, expected))
            return actual;
        
        throw Ex.Arg(actual, $"was not equal to {expected}", actualName);
    }
    
    public static T IsNotEqual<T>(T actual, T expected, IEqualityComparer<T>? comparer,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (comparer is null)
            return IsNotEqual<T>(actual, expected, actualName);
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            return actual;
        throw Ex.Arg(actual, $"was equal to {expected}", actualName);
    }

#if NET9_0_OR_GREATER
    public static T IsEqual<T>(T actual, T expected,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (Any.Equals(in actual, expected))
            return actual;
        throw Ex.Arg(actual, $"was not equal to {expected}", actualName);
    }
    
    public static T IsNotEqual<T>(T actual, T expected,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (!Any.Equals(in actual, expected))
            return actual;
        throw Ex.Arg(actual, $"was equal to {expected}", actualName);
    }
    
    public static T IsEqual<T>(T actual, T expected,
        IEqualityComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (comparer is null)
            return IsEqual<T>(actual, expected, _, actualName);
        if (comparer.Equals(actual, expected))
            return actual;
        throw Ex.Arg(actual, $"was not equal to {expected} according to {comparer}", actualName);
    }
    
    public static T IsNotEqual<T>(T actual, T expected,
        IEqualityComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (comparer is null)
            return IsNotEqual<T>(actual, expected, _, actualName);
        if (!comparer.Equals(actual, expected))
            return actual;
        throw Ex.Arg(actual, $"was equal to {expected} according to {comparer}", actualName);
    }
#endif
}