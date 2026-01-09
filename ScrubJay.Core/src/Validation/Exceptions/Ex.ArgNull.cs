namespace ScrubJay.Validation;

partial class Ex
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ArgumentNullException ArgNull(string? paramName)
    {
        return new ArgumentNullException(paramName);
    }
    
    public static ArgumentNullException ArgNull<T>(T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        string message = TextBuilder
            .New
            .Append("Argument ")
            .AppendArgument(argument, argumentName)
            .Append(" was not supposed to be null!")
            .ToStringAndDispose();
        return new ArgumentNullException(message);
    }
}