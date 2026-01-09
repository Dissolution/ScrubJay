namespace ScrubJay.Validation;

partial class Ex
{
    
    public static ArgumentOutOfRangeException ArgRange<T>(T? argument,
        [HandlesResourceDisposal] InterpolatedTextBuilder info = default,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        string message = info.ToStringAndClear();
        return new ArgumentOutOfRangeException(argumentName, argument, message);
    }
}