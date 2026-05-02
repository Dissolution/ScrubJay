#if !NET9_0_OR_GREATER
using Lock = System.Object;
#endif
using Dbg = System.Diagnostics.Debug;

namespace ScrubJay.Text.Debugging;

[PublicAPI]
internal static class Trouble
{
    private static readonly Lock _consoleLock = new Lock();
    private static readonly Lock _debugLock = new Lock();
    private static readonly Lock _debuggerLock = new Lock();
    private static readonly Lock _traceLock = new Lock();

    private static readonly ConsoleColor _defaultForeColor;

    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Hold() { }
    
    static Trouble()
    {
        lock (_consoleLock)
        {
            Console.ResetColor();
            _defaultForeColor = Console.ForegroundColor;
        }
        lock (_traceLock)
        {
            Trace.UseGlobalLock = true;
        }
    }

    private static void WriteToConsole(LogMessage log)
    {
        lock (_consoleLock)
        {
            Console.ForegroundColor = _defaultForeColor;
            Console.Write($"[{log.Timestamp:HH:mm:ss}] - ");
            Console.ForegroundColor = getColor(log.Level);
            Console.Write(log.Level);
            Console.ForegroundColor = _defaultForeColor;
            Console.WriteLine();

            if (log.CallerInfo is not null)
            {
                Console.Write("  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Caller: ");
                Console.ForegroundColor = _defaultForeColor;
                Console.Write(log.CallerInfo);
                Console.WriteLine();
            }

            if (!string.IsNullOrEmpty(log.Message))
            {
                Console.Write("  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Message: ");
                Console.ForegroundColor = _defaultForeColor;
                Console.Write(log.Message);
                Console.WriteLine();
            }

            if (log.Exception is not null)
            {
                Console.Write("  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(log.Exception.GetType().Name);
                Console.Write(": ");
                Console.ForegroundColor = _defaultForeColor;
                Console.Write(log.Exception.Message);
                Console.WriteLine();
            }
        }

        return;

        static ConsoleColor getColor(LogLevel level) => level switch
        {
            LogLevel.Debug => ConsoleColor.DarkGray,
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warn => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Fatal => ConsoleColor.Red,
            _ => ConsoleColor.Gray,
        };
    }

    [Conditional("DEBUG")]
    private static void WriteToDebug(LogMessage log)
    {
        lock (_debugLock)
        {
            Dbg.WriteLine($"[{log.Timestamp:HH:mm:ss}] - {log.Level}");

            if (log.CallerInfo is not null)
            {
                Dbg.Indent();
                Dbg.WriteLine($"Caller: {log.CallerInfo}");
                Dbg.Unindent();
            }

            if (!string.IsNullOrEmpty(log.Message))
            {
                Dbg.Indent();
                Dbg.WriteLine($"Message: {log.Message}");
                Dbg.Unindent();
            }

            if (log.Exception is not null)
            {
                Dbg.Indent();
                Dbg.WriteLine($"{log.Exception.GetType().Name}: {log.Exception.Message}");
                Dbg.Unindent();
            }
        }
    }
    
    private static void WriteToDebugger(LogMessage log)
    {
        lock (_debuggerLock)
        {
            if (!Debugger.IsLogging())
                return;
            
            var builder = new StringBuilder()
                .AppendLine($"[{log.Timestamp:HH:mm:ss}] - {log.Level}");

            if (log.CallerInfo is not null)
            {
                builder.AppendLine($"  Caller: {log.CallerInfo}");
            }

            if (!string.IsNullOrEmpty(log.Message))
            {
                builder.AppendLine($"  Message: {log.Message}");
            }

            if (log.Exception is not null)
            {
                builder.AppendLine($"  {log.Exception.GetType().Name}: {log.Exception.Message}");
            }

            var message = builder.ToString();

            Debugger.Log(
                level: (int)log.Level,
                category: nameof(Trouble),
                message: message);
        }
    }
    
    private static void WriteToTrace(LogMessage log)
    {
        lock (_traceLock)
        {
            var builder = new StringBuilder()
                .AppendLine($"[{log.Timestamp:HH:mm:ss}] - {log.Level}");

            if (log.CallerInfo is not null)
            {
                builder.AppendLine($"  Caller: {log.CallerInfo}");
            }

            if (!string.IsNullOrEmpty(log.Message))
            {
                builder.AppendLine($"  Message: {log.Message}");
            }

            if (log.Exception is not null)
            {
                builder.AppendLine($"  {log.Exception.GetType().Name}: {log.Exception.Message}");
            }

            var message = builder.ToString();

            switch (log.Level)
            {
                case LogLevel.Info:
                {
                    Trace.TraceInformation(message);
                    return;
                }
                case LogLevel.Warn:
                {
                    Trace.TraceWarning(message);
                    return;
                }
                case LogLevel.Error:
                case LogLevel.Fatal:
                {
                    Trace.TraceError(message);
                    return;
                }
                case LogLevel.Debug:
                default:
                {
                    Trace.WriteLine(message);
                    return;
                }
            }
        }
    }

    public static void Log(LogMessage logMessage)
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
        var log = new LogMessage(level, message, error, callerInfo);
        Log(log);
    }

    public static void Log(LogLevel level,
        string? message = null,
        [CallerLineNumber] int? callerLineNumber = null,
        [CallerFilePath] string? callerFilePath = null,
        [CallerMemberName] string? callerMemberName = null)
    {
        var callerInfo = CallerInfo.Capture(callerFilePath, callerLineNumber, callerMemberName);
        var log = new LogMessage(level, message, null, callerInfo);
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
}