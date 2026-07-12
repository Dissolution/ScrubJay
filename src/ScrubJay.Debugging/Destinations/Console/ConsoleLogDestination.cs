using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Debugging.Destinations;

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
            Console.OutputEncoding = Encoding.UTF8;

            DefaultColors = new(Console.ForegroundColor, Console.BackgroundColor);
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

    protected override void WriteImpl(LogEvent logEvent)
    {
        lock (_lock)
        {
            Console.Write('[');
            Console.Write(TimestampColors, logEvent.Timestamp.ToString("HH:mm:ss"));
            Console.Write("] - ");
            Console.Write(LogLevelColors.GetValueOrDefault(logEvent.Level), logEvent.Level.ToString());

            var message = logEvent.Message;
            if (message is not null)
            {
                Console.WriteLine();
                Console.Write(new string(' ', ParameterIndent));
                Console.Write(ParameterColors, "Message");
                Console.Write(": ");
                // todo
                Console.Write(MessageColors, message.ToString());
            }

            var exception = logEvent.Exception;
            if (exception is not null)
            {
                Console.WriteLine();
                Console.Write(new string(' ', ParameterIndent));
                Console.Write(ParameterColors, TypeName.For(exception));
                Console.Write(": ");
                Console.Write(MessageColors, exception.Message);
            }

            if (logEvent.Data.Count > 0)
            {
                Console.WriteLine();
                Console.Write(new string(' ', ParameterIndent));
                Console.Write(ParameterColors, "Data");
                Console.WriteLine(":");
                foreach (var pair in logEvent.Data)
                {
                    Console.WriteLine();
                    Console.Write(new string(' ', ParameterIndent*2));
                    Console.Write(ParameterColors, pair.Key);
                    Console.Write(": ");
                    Console.Write(MessageColors, pair.Value?.ToString());
                }
            }
            
            // final newline
            Console.WriteLine();
        }
    }
}