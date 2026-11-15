using ScrubJay.Debugging.Logging;
using ScrubJay.Generics.Extensions;

namespace ScrubJay.Debugging.Destinations.Console;

[PublicAPI]
public sealed class ConsoleLogDestination : LogDestination
{
    public ConsoleColors DefaultColors { get; set; }
    public ConsoleColorChange ParameterColors { get; set; }
    public ConsoleColorChange TimestampColors { get; set; }
    public Dictionary<LogLevel, ConsoleColorChange> LogLevelColors { get; }
    public ConsoleColorChange MessageColors { get; set; }

    public int ParameterIndent { get; set; }

    public ConsoleLogDestination()
    {
        lock (_lock)
        {
            System.Console.OutputEncoding = Encoding.UTF8;

            DefaultColors = new(System.Console.ForegroundColor, System.Console.BackgroundColor);
            ParameterColors = ConsoleColor.White;
            TimestampColors = ConsoleColor.Blue;
            LogLevelColors = new()
            {
                { LogLevel.Info, ConsoleColor.White },
                { LogLevel.Warn, ConsoleColor.Yellow },
                { LogLevel.Error, ConsoleColor.Red },
                { LogLevel.Fatal, ConsoleColor.Magenta },
            };
            MessageColors = default;
            ParameterIndent = 2;
        }
    }

    protected override void WriteCore(LogEvent logEvent)
    {
        lock (_lock)
        {
            System.Console.Write('[');
            System.Console.Write(TimestampColors, logEvent.Timestamp.ToString("HH:mm:ss"));
            System.Console.Write("] - ");
            System.Console.Write(LogLevelColors.GetValueOrDefault(logEvent.Level), logEvent.Level.ToString());

            var message = logEvent.Message;
            if (message is not null)
            {
                System.Console.WriteLine();
                System.Console.Write(new string(' ', ParameterIndent));
                System.Console.Write(ParameterColors, "Message");
                System.Console.Write(": ");
                // todo
                System.Console.Write(MessageColors, message.ToString());
            }

            var exception = logEvent.Exception;
            if (exception is not null)
            {
                System.Console.WriteLine();
                System.Console.Write(new string(' ', ParameterIndent));
                System.Console.Write(ParameterColors, exception.GetType().Alias);
                System.Console.Write(": ");
                System.Console.Write(MessageColors, exception.Message);
            }

            if (logEvent.Data.Count > 0)
            {
                System.Console.WriteLine();
                System.Console.Write(new string(' ', ParameterIndent));
                System.Console.Write(ParameterColors, "Data");
                System.Console.WriteLine(":");
                foreach (var pair in logEvent.Data)
                {
                    System.Console.WriteLine();
                    System.Console.Write(new string(' ', ParameterIndent * 2));
                    System.Console.Write(ParameterColors, pair.Key);
                    System.Console.Write(": ");
                    System.Console.Write(MessageColors, pair.Value?.ToString());
                }
            }

            // final newline
            System.Console.WriteLine();
        }
    }
}