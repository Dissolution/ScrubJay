using ScrubJay.Extensions;
using ScrubJay.Text;

namespace ScrubJay.Errors;

[PublicAPI]
[StackTraceHidden]
public static partial class Ex
{
    internal static string CreateMessage(
        [HandlesResourceDisposal] ref Interpolator message,
        string? additionalInfo = null)
    {
        if (additionalInfo.IsNotEmpty)
        {
            // cut off a trailing .
            message.TrimLast('.');

            // add the additional info
            message.Write(": ");
            message.Write(additionalInfo);
        }

        // ensure message ends with punctuation
        if (message.Written.TryGetLast(out var last) && !char.IsEndOfSentencePunctuation(last))
        {
            message.Write('.');
        }

        return message.ToStringAndDispose();
    }
}