namespace ScrubJay.Validation;

partial class Ex
{
    public static ArgumentOutOfRangeException ArgRange<T>(
        T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetArgExceptionMessage<T>(argument, argumentName, info);
        if (!Any.TryBox(argument, out var boxed))
            boxed = Any.ToString(in argument);
        return new ArgumentOutOfRangeException(argumentName, boxed, message);
    }

    public static ArgumentOutOfRangeException ArgRange<T>(
        T? argument,
        ref InterpolatedTextBuilder info,
        TypeConstraints.AllowsRefStruct<T> _ = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetArgExceptionMessage<T>(argument, argumentName, ref info);
        if (!Any.TryBox(argument, out var boxed))
            boxed = Any.ToString(in argument);
        return new ArgumentOutOfRangeException(argumentName, boxed, message);
    }

    
    public static ArgumentOutOfRangeException ArgRange<T>(
        T? argument,
        T? allowed,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetArgExceptionMessage<T>(argument, argumentName, $"Must be `{allowed}`");
        return new ArgumentOutOfRangeException(argumentName, BoxOrToStringArg(argument), message);
    }
    
    public static ArgumentOutOfRangeException ArgRange<T>(
        T? argument,
        IEnumerable<T> allowed,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = GetArgExceptionMessage<T>(argument, argumentName, $"Must be `{allowed}`");
        return new ArgumentOutOfRangeException(argumentName, BoxOrToStringArg(argument), message);
    }
    
    public static ArgumentOutOfRangeException ArgRange<T>(
        T? argument,
        LowerBound<T> lowerBound,
        UpperBound<T> upperBound,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        var message = GetArgExceptionMessage<T>(argument, argumentName, $"Must be in {Bounds.GetString<T>(lowerBound, upperBound)}");
        return new ArgumentOutOfRangeException(argumentName, BoxOrToStringArg(argument), message);
    }
}