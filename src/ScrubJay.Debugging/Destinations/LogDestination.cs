using ScrubJay.Debugging.Logging;

namespace ScrubJay.Debugging.Destinations;

public abstract class LogDestination
{
    internal readonly Lock _lock = new Lock();

    public LogLevel MinLogLevel { get; set; } = LogLevel.Info;

    protected virtual bool ShouldWrite(LogEvent logEvent) => true;
    protected abstract void WriteImpl(LogEvent logEvent);

    public void Write(LogEvent logEvent)
    {
        if (logEvent.Level < MinLogLevel) return;
        if (!ShouldWrite(logEvent)) return;
        WriteImpl(logEvent);
    }
}