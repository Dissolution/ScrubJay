using System.Reflection;
using ScrubJay.Polyfills.Collections;
using ScrubJay.Polyfills.Text;
using ScrubJay.Polyfills;

namespace ScrubJay.Errors.Utilities;

internal static class ExceptionRenderer
{
    [return: NotNullIfNotNull(nameof(exception))]
    public static string? RenderException<TException>(
        TException? exception,
        InterpolatedTextWrite<TException>? writeAdditionalInformation)
        where TException : Exception
    {
        if (exception is null)
            return null;

        var text = new InterpolatedText();
        text.Write(exception.GetType());
        text.Write(": ");

        if (exception.Message.IsNotEmpty())
        {
            text.NewLine();
            text.Write("  Message: \"");
            text.Write(exception.Message);
            text.Write('"');
        }

        if (writeAdditionalInformation is not null)
        {
            writeAdditionalInformation(ref text, exception);
        }

        string? source = exception.Source;
        MethodBase? targetSite = exception.TargetSite;

        if (source is not null || targetSite is not null)
        {
            text.NewLine();
            text.Write("  Source Target: ");
            if (source is not null)
            {
                text.Write(source);
                if (targetSite is not null)
                {
                    text.Write('.');
                }
            }
            if (targetSite is not null)
            {
                text.Write(targetSite);
            }
        }
        
        string? stackTrace = exception.StackTrace;

        if (stackTrace is not null)
        {
            text.NewLine();
            text.Write("  StackTrace: ");
            text.Write(stackTrace);
        }

        // HResult
        var hResult = (HResult)exception.HResult;
        builder
            .NewLine()
            .Append($"HResult: {hResult:X} ({(hResult.IsSuccess ? "Success" : "Failure")})")
            // HelpLink
            .IfNotEmpty(exception.HelpLink,
                static (tb, hl) => tb.NewLine().Append($"HelpLink: {hl}"))
            // Data
            .IfNotEmpty(exception.Data, static (tb, data) =>
            {
                tb.NewLine()
                    .Append("Data:")
                    .Indent()
                    .NewLine()
                    .Delimit(
                        TB.NewLine,
                        data.OfType<DictionaryEntry>(),
                        static (t, entry) => t.Append($"{entry.Key:@}: {entry.Value}:@"))
                    .Outdent();
            })
            // Inner Exception(s)
            .IfNotNull(exception.InnerException, static (tb, inner) =>
            {
                tb.NewLine()
                    .Append("Inner ")
                    .Render(inner);
            });
        
    }

}