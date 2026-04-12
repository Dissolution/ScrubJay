#pragma warning disable RS1035

using System.Diagnostics;
using System.Text;
using SGF.Diagnostics;

namespace ScrubJay.Enums.SourceGen.Utilities;

public sealed class DebuggingLogger : ILogger
{
    public LogLevel MinLogLevel { get; set; }

    public bool IsEnabled(LogLevel level) => level >= MinLogLevel;

    public DebuggingLogger(LogLevel minLogLevel = LogLevel.Debug)
    {
        this.MinLogLevel = minLogLevel;
    }

    void ILogger.AddSink(ILogSink sink)
    {
        throw new NotSupportedException();
    }

    public void Log(LogLevel logLevel, Exception? exception, string? info)
    {
        StringBuilder builder = new StringBuilder()
            .Append($"[{DateTime.Now:HH:mm:ss}] {logLevel}");
        if (!string.IsNullOrEmpty(info))
        {
            builder.Append(": ").Append(info);
        }
        if (exception is not null)
        {
            builder.AppendLine()
                .Append($"  {exception.GetType().Name}: {exception.Message}");
        }
        string message = builder.ToString();
        Console.WriteLine(message);
        Debug.WriteLine(message, category: "SourceGen");
    }
}