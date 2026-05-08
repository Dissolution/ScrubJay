namespace ScrubJay.Errors;

partial class Ex
{
    public static ArgException Convert<I>(
        I? input,
        Type? destinationType,
        string? info = null,
        [CallerArgumentExpression(nameof(input))]
        string? argumentName = null)
    {
        var arg = Argument.Capture<I>(in input, argumentName);
        var message = TextBuilder.Rent()
            .Append($"Argument {arg:@} could not be converted to a {destinationType:@}")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgException(arg, message);
    }
}