namespace ScrubJay.Debugging.Logging;

[PublicAPI]
public static class LogEventExtensions
{
    extension(LogEvent? logEvent)
    {
        public void AddCallerInfo(CallerInfo? callerInfo)
        {
            if (logEvent is null || callerInfo is null)
                return;
            logEvent.Add("CallerFilePath", callerInfo.FilePath);
            logEvent.Add("CallerLineNumber", callerInfo.LineNumber);
            logEvent.Add("CallerMemberName", callerInfo.MemberName);
        }

        public void AddStackTrace(StackTrace? stackTrace)
        {
            if (logEvent is null || stackTrace is null)
                return;
            logEvent.Add("StackTrace", stackTrace);
        }
    }
}