namespace ScrubJay.Exceptions;

internal enum LogLevel
{
    Debug,
    Info,
    Warn,
    Error,
    Fatal,
}

internal readonly record struct LogMessage
{
    public readonly DateTime Timestamp;
    public readonly LogLevel Level;
    public readonly string? Message;
    public readonly Exception? Exception;
    public readonly string? Caller;

    public LogMessage(DateTime timestamp, LogLevel level, string? message, Exception? exception, string? caller)
    {
        Timestamp = timestamp;
        Level = level;
        Message = message;
        Exception = exception;
        Caller = caller;
    }
}

internal static class Log
{
    private static readonly object _consoleLock = new object();
    private static readonly object _debugLock = new object();
    private static readonly object _traceLock = new object();

    private static readonly ConsoleColor _defaultForeColor;

    static Log()
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

            if (!string.IsNullOrEmpty(log.Caller))
            {
                Console.Write("  ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Caller: ");
                Console.ForegroundColor = _defaultForeColor;
                Console.Write(log.Caller);
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

    private static void WriteToDebug(LogMessage log)
    {
        lock (_debugLock)
        {
            Debug.WriteLine($"[{log.Timestamp:HH:mm:ss}] - {log.Level}");

            if (!string.IsNullOrEmpty(log.Caller))
            {
                Debug.Indent();
                Debug.WriteLine($"Caller: {log.Caller}");
                Debug.Unindent();
            }

            if (!string.IsNullOrEmpty(log.Message))
            {
                Debug.Indent();
                Debug.WriteLine($"Message: {log.Message}");
                Debug.Unindent();
            }

            if (log.Exception is not null)
            {
                Debug.Indent();
                Debug.WriteLine($"{log.Exception.GetType().Name}: {log.Exception.Message}");
                Debug.Unindent();
            }
        }
    }

    private static void WriteToTrace(LogMessage log)
    {
        lock (_traceLock)
        {
            var builder = new StringBuilder()
                .AppendLine($"[{log.Timestamp:HH:mm:ss}] - {log.Level}");

            if (!string.IsNullOrEmpty(log.Caller))
            {
                builder.AppendLine($"  Caller: {log.Caller}");
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

    public static void Write(LogMessage log)
    {
        WriteToConsole(log);
        WriteToDebug(log);
        WriteToTrace(log);
    }

    public static void Write(LogLevel level, string? message, [CallerMemberName] string? caller = null)
    {
        var log = new LogMessage(DateTime.Now, level, message, null, caller);
        Write(log);
    }

    public static void Write(LogLevel level, Exception? ex, [CallerMemberName] string? caller = null)
    {
        var log = new LogMessage(DateTime.Now, level, null, ex, caller);
        Write(log);
    }

    public static void Write(LogLevel level, string? message, Exception? ex, [CallerMemberName] string? caller = null)
    {
        var log = new LogMessage(DateTime.Now, level, message, ex, caller);
        Write(log);
    }
}