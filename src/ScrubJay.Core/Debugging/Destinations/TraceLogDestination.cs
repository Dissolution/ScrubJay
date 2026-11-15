using ScrubJay.Debugging.Logging;
using ScrubJay.Generics.Extensions;

namespace ScrubJay.Debugging.Destinations;

[PublicAPI]
public sealed class TraceLogDestination : LogDestination
{
    public TraceLogDestination()
    {
        Trace.UseGlobalLock = true;
    }

    private string GetLogMessage(LogEvent logEvent)
    {
        var builder = new StringBuilder()
            .Append($"[{logEvent.Timestamp:HH:mm:ss}] - {logEvent.Level}");

        var message = logEvent.Message;
        if (message is not null)
        {
            builder.AppendLine()
                .Append($"  Message: {message}");
        }

        var exception = logEvent.Exception;
        if (exception is not null)
        {
            builder.AppendLine()
                .Append($"  {exception.GetType().Alias}: {exception.Message}");
        }

        var data = logEvent.Data;
        if (data.Count > 0)
        {
            builder.AppendLine()
                .Append("  Data:");
            foreach (var pair in data)
            {
                builder.AppendLine()
                    .Append($"    {pair.Key}: {pair.Value}");
            }
        }
        
        return builder.ToString();
    }

    protected override bool ShouldWrite(LogEvent logEvent) => Trace.Listeners.Count > 0;

    protected override void WriteCore(LogEvent logEvent)
    {
        var logMessage = GetLogMessage(logEvent);
        
        lock (_lock)
        {
            switch (logEvent.Level)
            {
                case LogLevel.Info:
                {
                    Trace.TraceInformation(logMessage);
                    return;
                }
                case LogLevel.Warn:
                {
                    Trace.TraceWarning(logMessage);
                    return;
                }
                case LogLevel.Error:
                case LogLevel.Fatal:
                {
                    Trace.TraceError(logMessage);
                    return;
                }
                default:
                {
                    Trace.WriteLine(logMessage);
                    return;
                }
            }
        }
    }
}