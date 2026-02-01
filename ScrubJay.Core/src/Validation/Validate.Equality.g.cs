#nullable enable

namespace ScrubJay.Validation;

partial class Validate
{
    public static Result<T> IsEqual<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (EqualityComparer<T>.Default.Equals(actual, expected))
            return actual;
        return Ex.Arg(actual, $"was not equal to {expected}", actualName);
    }
    
    public static Result<T> IsNotEqual<T>(T actual, T expected,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            return actual;
        return Ex.Arg(actual, $"was equal to {expected}", actualName);
    }
    
    public static Result<T> IsEqual<T>(T actual, T expected, IEqualityComparer<T>? comparer,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (comparer is null)
            return IsEqual<T>(actual, expected, actualName);
        
        if (comparer.Equals(actual, expected))
            return actual;
        
        return Ex.Arg(actual, $"was not equal to {expected}", actualName);
    }
    
    public static Result<T> IsNotEqual<T>(T actual, T expected, IEqualityComparer<T>? comparer,
        [CallerArgumentExpression(nameof(actual))]
        string? actualName = null)
    {
        if (comparer is null)
            return IsNotEqual<T>(actual, expected, actualName);
        if (!EqualityComparer<T>.Default.Equals(actual, expected))
            return actual;
        return Ex.Arg(actual, $"was equal to {expected}", actualName);
    }

#if NET9_0_OR_GREATER
    public static RefResult<T> IsEqual<T>(T actual, T expected,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (Any.Equals(actual, expected))
            return actual;
        return Ex.Arg(actual, $"was not equal to {expected}", actualName);
    }
    
    public static RefResult<T> IsNotEqual<T>(T actual, T expected,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (!Any.Equals(actual, expected))
            return actual;
        return Ex.Arg(actual, $"was equal to {expected}", actualName);
    }
    
    public static RefResult<T> IsEqual<T>(T actual, T expected,
        IEqualityComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (comparer is null)
            return IsEqual<T>(actual, expected, _, actualName);
        if (comparer.Equals(actual, expected))
            return actual;
        return Ex.Arg(actual, $"was not equal to {expected} according to {comparer}", actualName);
    }
    
    public static RefResult<T> IsNotEqual<T>(T actual, T expected,
        IEqualityComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : allows ref struct
    {
        if (comparer is null)
            return IsNotEqual<T>(actual, expected, _, actualName);
        if (!comparer.Equals(actual, expected))
            return actual;
        return Ex.Arg(actual, $"was equal to {expected} according to {comparer}", actualName);
    }
#endif
}
