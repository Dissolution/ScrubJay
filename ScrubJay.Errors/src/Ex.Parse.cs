namespace ScrubJay.Errors;

partial class Ex
{
    public static ParseException Parse<T>(
        string? input,
        string? info = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = TextBuilder.Rent()
            .Append($"Could not parse \"{input}\" into a {typeof(T):@} value")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ParseException(input, typeof(T), message);
    }
    
    public static ParseException Parse<T>(
        scoped text input,
        string? info = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var message = TextBuilder.Rent()
            .Append($"Could not parse \"{input}\" into a {typeof(T):@} value")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ParseException(input.ToString(), typeof(T), message);
    }
}