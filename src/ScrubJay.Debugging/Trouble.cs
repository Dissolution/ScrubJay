using Dbg = System.Diagnostics.Debug;


namespace ScrubJay.Debugging;

[PublicAPI]
public static class Trouble
{
    private static readonly Lock _debuggerLock = new Lock();
    private static readonly Lock _traceLock = new Lock();

    private static readonly ConsoleColor _defaultForeColor;

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Hold() { }

    [DebuggerHidden]
    [StackTraceHidden]
    public static void Break()
    {
        Debugger.Break();
    }


    /*


    public static void Log(LogEvent logMessage)
    {
        WriteToConsole(logMessage);
        WriteToDebug(logMessage);
        WriteToDebugger(logMessage);
        WriteToTrace(logMessage);
    }

    public static void Log(LogLevel level,
        Exception? error,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
    {
        var callerInfo = CallerInfo.Capture(callerFilePath, callerLineNumber, callerMemberName);
        var log = new LogEvent(level, message, error, callerInfo);
        Log(log);
    }

    public static void Log(LogLevel level,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
    {
        var callerInfo = CallerInfo.Capture(callerFilePath, callerLineNumber, callerMemberName);
        var log = new LogEvent(level, message, null, callerInfo);
        Log(log);
    }

    public static void Debug(
        Exception? error,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Debug, error, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Debug(
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Debug, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Info(
        Exception? error,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Info, error, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Info(
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Info, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Warn(
        Exception? error,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Warn, error, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Warn(
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Warn, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Error(
        Exception? error,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Error, error, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Error(
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Error, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Fatal(
        Exception? error,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Fatal, error, message, callerLineNumber, callerFilePath, callerMemberName);

    public static void Fatal(
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
        => Log(LogLevel.Fatal, message, callerLineNumber, callerFilePath, callerMemberName);
        
        */
}