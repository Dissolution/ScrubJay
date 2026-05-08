namespace ScrubJay.Errors.Validation;

partial class Validate
{
    public static Result<T?> Equal<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(argument, expected))
            return Ex.ArgNotEqual<T>(argument, expected, info, argumentName);
        return argument;
    }

    public static Result<T?> NotEqual<T>(T? argument, T? expected,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        if (EqualityComparer<T>.Default.Equals(argument, expected))
            return Ex.ArgEqual<T>(argument, expected, info, argumentName);
        return argument;
    }
}