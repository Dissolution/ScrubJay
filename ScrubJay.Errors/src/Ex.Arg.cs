#pragma warning disable CS8764

namespace ScrubJay.Errors;

public partial class Ex
{
    /// <summary>
    /// Returns a new <see cref="ArgException"/> with a preset Message.
    /// </summary>
    /// <param name="argument"></param>
    /// <param name="info"></param>
    /// <param name="argumentName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ArgException Arg<T>(
        in T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        var message = TextBuilder.Rent()
            .Append("Argument ")
            .Render(arg)
            .Append(" was invalid")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgException(arg, message);
    }
    
    public static ArgException Arg(Argument? arg,
        string? info = null,
        Exception? innerException = null)
    {
        arg ??= Argument.Null();
        var message = TextBuilder.Rent()
            .Append("Argument ")
            .Render(arg)
            .Append(" was invalid")
            .AppendInfo(info)
            .ToStringAndDispose();
        return new ArgException(arg, message, innerException);
    }
}