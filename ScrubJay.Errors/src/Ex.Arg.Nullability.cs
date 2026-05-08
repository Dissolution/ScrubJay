namespace ScrubJay.Errors;

partial class Ex
{
    public static ArgNullException ArgNull<T>(
        T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = TextBuilder.Rent()
            .Append("Argument ")
            .IfNotEmpty(argumentName, static (tb, name) => tb.Append($"'{name}' )"))
            .Append($"({typeof(T):@}) was null")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgNullException(Argument.Null<T>(argumentName), message);
    }
}