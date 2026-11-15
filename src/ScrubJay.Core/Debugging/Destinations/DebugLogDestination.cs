using ScrubJay.Debugging.Logging;
using ScrubJay.Generics.Extensions;

namespace ScrubJay.Debugging.Destinations;

public sealed class DebugLogDestination : LogDestination
{
    public DebugLogDestination()
    {
    }

    protected override void WriteCore(LogEvent logEvent)
    {
        lock (_lock)
        {
            Debug.WriteLine($"[{logEvent.Timestamp:HH:mm:ss}] - {logEvent.Level}");

            var message = logEvent.Message;
            if (message is not null)
            {
                Debug.Indent();
                Debug.WriteLine($"Message: {message}");
                Debug.Unindent();
            }

            var exception = logEvent.Exception;
            if (exception is not null)
            {
                Debug.Indent();
                Debug.WriteLine($"{exception.GetType().Alias}: {exception.Message}");
                Debug.Unindent();
            }

            var data = logEvent.Data;
            if (data.Count > 0)
            {
                Debug.Indent();
                Debug.WriteLine("Data:");
                Debug.Indent();
                foreach (var pair in data)
                {
                    Debug.WriteLine($"{pair.Key}: {pair.Value}");
                }
                Debug.Unindent();
                Debug.Unindent();
            }
        }
    }
}