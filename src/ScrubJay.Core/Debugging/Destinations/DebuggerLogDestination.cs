using ScrubJay.Debugging.Logging;
using ScrubJay.Generics.Extensions;

namespace ScrubJay.Debugging.Destinations;

[PublicAPI]
public sealed class DebuggerLogDestination : LogDestination
{
    protected override bool ShouldWrite(LogEvent logEvent) => Debugger.IsLogging();

    protected override void WriteCore(LogEvent logEvent)
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
            
            var logMessage = builder.ToString();
            
            Debugger.Log(
                level: (int)logEvent.Level,
                category: nameof(Trouble),
                message: logMessage);
        }
    }
}