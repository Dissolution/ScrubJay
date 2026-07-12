using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Debugging.Destinations;

[PublicAPI]
public sealed class TraceLogDestination : LogDestination
{
    public TraceLogDestination()
    {
        Trace.UseGlobalLock = true;
    }

    protected override bool ShouldWrite(LogEvent logEvent) => Trace.Listeners.Count > 0;

    protected override void WriteImpl(LogEvent logEvent)
    {
        lock (_lock)
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
                    .Append($"  {TypeName.For(exception)}: {exception.Message}");
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
            
            var logMessage = builder.ToString();
            
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