namespace ScrubJay.Debugging;

internal sealed class LogMessage
{
    public required DateTime Timestamp { get; init; }

    public required LogLevel Level { get; init; }

    public required string? Message { get; init; }

    public Exception? Exception { get; init; }

    public CallerInfo? CallerInfo { get; init; }

    public LogMessage()
    {
    }

    [SetsRequiredMembers]
    public LogMessage(
        LogLevel level,
        string? message,
        Exception? exception = null,
        CallerInfo? callerInfo = null)
    {
        Timestamp = DateTime.Now;
        Level = level;
        Message = message;
        Exception = exception;
        CallerInfo = callerInfo;
    }

    [SetsRequiredMembers]
    public LogMessage(DateTime timestamp,
        LogLevel level,
        string? message,
        Exception? exception = null,
        CallerInfo? callerInfo = null)
    {
        Timestamp = timestamp;
        Level = level;
        Message = message;
        Exception = exception;
        CallerInfo = callerInfo;
    }
}