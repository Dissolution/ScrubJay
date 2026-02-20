namespace ScrubJay.Validation;

partial class Ex
{
    public static ArgumentOutOfRangeException ArgRange<T>(T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new ArgumentOutOfRangeException(argumentName, argument, null);
    }

    public static ArgumentOutOfRangeException ArgRange<T>(T? argument,
        [HandlesResourceDisposal] InterpolatedTextBuilder info,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        string message = info.ToStringAndClear();
        return new ArgumentOutOfRangeException(argumentName, argument, message);
    }

#if NET9_0_OR_GREATER
    public static ArgumentOutOfRangeException ArgRange<T>(T? argument,
        TypeConstraints.AllowsRefStruct<T> _ = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        where T : allows ref struct
    {
        return new ArgumentOutOfRangeException(argumentName, Any.TryBox(argument, out var boxed) ? boxed : Any.ToString(argument), null);
    }

    public static ArgumentOutOfRangeException ArgRange<T>(T? argument,
        [HandlesResourceDisposal] InterpolatedTextBuilder info,
        TypeConstraints.AllowsRefStruct<T> _ = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        where T : allows ref struct
    {
        string message = info.ToStringAndClear();
        return new ArgumentOutOfRangeException(argumentName, Any.TryBox(argument, out var boxed) ? boxed : Any.ToString(argument), message);
    }


#endif
}