namespace ScrubJay.Validation;

partial class Ex
{
    public static ArgumentNullException ArgNull<T>(string? argumentName)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        string message = TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argumentName, typeof(T))
            .Append(" is null")
            .ToStringAndDispose();

        return new ArgumentNullException(argumentName, message);
    }
    
    public static ArgumentNullException ArgNull<T>(
        T? argument,
        string? info,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        string message = TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argumentName, typeof(T), null)
            .Append(" is null")
            .AppendOptionalInfo(info)
            .ToStringAndDispose();

        return new ArgumentNullException(argumentName, message);
    }

    public static ArgumentNullException ArgNull<T>(
        T? argument,
        ref InterpolatedTextBuilder info,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        string message = TextBuilder.New
            .Append("Argument ")
            .AppendArgument(argumentName, typeof(T), null)
            .Append(" is null")
            .AppendOptionalInfo(ref info)
            .ToStringAndDispose();

        return new ArgumentNullException(argumentName, message);
    }
}