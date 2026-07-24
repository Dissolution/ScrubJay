//using System.Reflection;
//
//namespace ScrubJay.Errors.Utilities;
//
//internal static class ExceptionRenderer
//{
//    private static void WriteExceptionTo<TException>(
//        ref InterpolatedText text,
//        TException? exception,
//        InterpolatedTextWrite<TException, int>? writeAdditionalInformation,
//        int indent)
//        where TException : Exception
//    {
//        if (exception is null) return;
//
//        text.Write(exception.GetType());
//        text.Write(": ");
//
//        if (exception.Message.IsNotEmpty())
//        {
//            text.NewLine();
//            text.Fill(indent * 2, ' ');
//            text.Write("Message: \"");
//            text.Write(exception.Message);
//            text.Write('"');
//        }
//
//        if (writeAdditionalInformation is not null)
//        {
//            writeAdditionalInformation(ref text, exception, indent);
//        }
//
//        string? source = exception.Source;
//        MethodBase? targetSite = exception.TargetSite;
//
//        if (source is not null || targetSite is not null)
//        {
//            text.NewLine();
//            text.Fill(indent * 2, ' ');
//            text.Write("Source Target: ");
//            if (source is not null)
//            {
//                text.Write(source);
//                if (targetSite is not null)
//                {
//                    text.Write('.');
//                }
//            }
//            if (targetSite is not null)
//            {
//                text.Write(targetSite);
//            }
//        }
//
//        string? stackTrace = exception.StackTrace;
//
//        if (stackTrace is not null)
//        {
//            text.NewLine();
//            text.Fill(indent * 2, ' ');
//            text.Write("StackTrace: ");
//            text.Write(stackTrace);
//        }
//
//        // HResult
//        var hResult = (HResult)exception.HResult;
//        text.NewLine();
//        text.Fill(indent * 2, ' ');
//        text.Write("HResult: ");
//        text.Format(hResult, "X");
//        text.Write(" (");
//        if (hResult.IsSuccess)
//        {
//            text.Write("Success");
//        }
//        else
//        {
//            text.Write("Failure");
//        }
//        text.Write(")");
//
//        // HelpLink
//        if (exception.HelpLink.IsNotEmpty())
//        {
//            text.NewLine();
//            text.Fill(indent * 2, ' ');
//            text.Write("HelpLink: ");
//            text.Write(exception.HelpLink);
//        }
//
//        // Data
//        if (exception.Data.IsNotEmpty())
//        {
//            text.NewLine();
//            text.Fill(indent * 2, ' ');
//            text.Write(exception.Data.Count);
//            text.Write("x Data:");
//
//            indent++;
//            
//            foreach (DictionaryEntry entry in exception.Data)
//            {
//                text.NewLine();
//                text.Fill(indent * 2, ' ');
//                text.Write(entry.Key);
//                text.Write(": ");
//                text.Write(entry.Value);
//            }
//
//            indent--;
//        }
//
//        if (exception.InnerException is not null)
//        {
//            text.NewLine();
//            text.Fill(indent * 2, ' ');
//            text.Write("InnerException: ");
//            WriteExceptionTo(ref text, exception.InnerException, null, indent + 1);
//        }
//    }
//
//    [return: NotNullIfNotNull(nameof(exception))]
//    public static string? RenderException<TException>(
//        TException? exception,
//        InterpolatedTextWrite<TException, int>? writeAdditionalInformation)
//        where TException : Exception
//    {
//        if (exception is null)
//            return null;
//
//        var text = new InterpolatedText();
//        WriteExceptionTo(ref text, exception, writeAdditionalInformation, 1);
//        return text.ToStringAndDispose();
//    }
//}