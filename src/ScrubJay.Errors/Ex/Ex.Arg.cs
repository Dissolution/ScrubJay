using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Exceptions;
using ScrubJay.Polyfills;
using ScrubJay.Polyfills.Text;

namespace ScrubJay.Errors;

public partial class Ex
{
    public static ArgException Arg<T>(
        in T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = ArgumentInfo.Capture<T>(in argument, argumentName);
        return Arg(arg, info);
    }

    public static ArgException Arg(
        ArgumentInfo argumentInfo,
        string? info = null)
    {
        using var message = new InterpolatedText();
        message.Write("ArgException: ");
        message.Write(argumentInfo);
        message.Write(" was invalid");
        if (info.IsNotEmpty())
        {
            message.Write(": ");
            message.Write(info);
        }
        return new ArgException(argumentInfo, message.ToString());
    }
}