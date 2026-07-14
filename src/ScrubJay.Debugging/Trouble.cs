using ScrubJay.Debugging.Destinations;


namespace ScrubJay.Debugging;

[PublicAPI]
public static class Trouble
{
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Hold() { }

    [DebuggerHidden]
    [StackTraceHidden]
    public static void Break()
    {
        Debugger.Break();
    }

    public static List<LogDestination> LogDestinations { get; } =
    [
        new ConsoleLogDestination(),
        new DebugLogDestination(),
        new DebuggerLogDestination(),
        new TraceLogDestination(),
    ];

    public static void Log(LogEvent logEvent)
    {
        foreach (var dest in LogDestinations)
        {
            dest.Write(logEvent);
        }
    }

    public static void Log(
        LogLevel level, 
        LogMessage? message, 
        Exception? exception = null)
        => Log(new LogEvent(level, message, exception));
    
    public static void Log(
        LogLevel level, 
        [HandlesResourceDisposal] LogMessageBuilder logMessage,
        Exception? exception = null)
        => Log(new LogEvent(level, logMessage, exception));
    
    public static void Log(
        LogLevel level, 
        Exception? exception)
        => Log(new LogEvent(level, exception, null));
    
    public static void Log(
        LogLevel level, 
        Exception? exception,
        LogMessage? message)
        => Log(new LogEvent(level, exception, message));
    
    public static void Log(
        LogLevel level, 
        Exception? exception,
        [HandlesResourceDisposal] LogMessageBuilder logMessage)
        => Log(new LogEvent(level, exception, logMessage));
  
}