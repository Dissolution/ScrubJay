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
            boxed = Any.ToString(argument);
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
            boxed = Any.ToString(argument);
        return new ArgumentOutOfRangeException(argumentName, boxed, message);
    }

}