namespace ScrubJay.Debugging;

[PublicAPI]
public sealed class LogEvent
{
    public required DateTime Timestamp { get; init; }

    public required LogLevel Level { get; init; }

    public LogMessage? Message { get; init; }

    public Exception? Exception { get; init; }

    public IReadOnlyDictionary<string, object?> Data { get; init; } = new Dictionary<string, object?>(capacity: 0);

    public LogEvent()
    {
    }

    [SetsRequiredMembers]
    public LogEvent(LogLevel level)
    {
        Timestamp = DateTime.Now;
        Level = level;
    }

    [SetsRequiredMembers]
    public LogEvent(
        DateTime timestamp,
        LogLevel level)
    {
        Timestamp = timestamp;
        Level = level;
    }
}