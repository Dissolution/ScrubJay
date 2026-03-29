using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Logging;

namespace ScrubJay.Enhancements.BuildTasks;

internal static class Extensions
{
    extension(TaskLoggingHelper log)
    {
        
    }
}


internal sealed class TaskLoggingHelperLogger<T> : ILogger<T>
    where T : ITask
{
    private static MessageImportance GetMessageImportance(LogLevel logLevel)
    {
        switch (logLevel)
        {
            case LogLevel.Critical:
            case LogLevel.Error:
                return MessageImportance.High;
            case LogLevel.Warning:
            case LogLevel.Information:
                return MessageImportance.Normal;
            case LogLevel.Debug:
            case LogLevel.Trace:
            case LogLevel.None:
            default:
                return MessageImportance.Low;
        }
    }
    
    private readonly TaskLoggingHelper _helper;

    public bool IsEnabled(LogLevel logLevel)
    {
        return _helper.LogsMessagesOfImportance(GetMessageImportance(logLevel));
    }
    
    public TaskLoggingHelperLogger(TaskLoggingHelper helper)
    {
        _helper = helper;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        switch (logLevel)
        {
            case LogLevel.Critical:
                break;
            case LogLevel.Error:
            {
                if (exception is not null)
                {
                    _helper.LogErrorFromException(exception, true, true, null);
                    return;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            case LogLevel.Warning:
            {
                if (exception is not null)
                {
                    _helper.LogWarningFromException(exception, true);
                    return;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            case LogLevel.Information:
                break;
            case LogLevel.Debug:
                break;
            case LogLevel.Trace:
                break;
            case LogLevel.None:
                break;
            default:
                break;
        }

        throw new NotImplementedException();
    }



    public IDisposable? BeginScope<TState>(TState state) 
        where TState : notnull
    {
        throw new NotImplementedException();
    }
}