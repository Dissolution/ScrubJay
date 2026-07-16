namespace ScrubJay.Debugging.Logging;

[PublicAPI]
public sealed class LogEvent : IEnumerable
{
    public DateTime Timestamp { get; set; }

    public LogLevel Level { get; set; }

    public LogMessage? Message { get; set; }

    public Exception? Exception { get; set; }

    public Dictionary<string, object?> Data { get; }

    public LogEvent()
    {
        this.Timestamp = DateTime.Now;
        this.Level = LogLevel.Info;
        this.Message = null;
        this.Exception = null;
        this.Data = new(capacity: 0);
    }

    public LogEvent(
        LogLevel level, 
        LogMessage? message, 
        Exception? exception = null)
    {
        Timestamp = DateTime.Now;
        Level = level;
        Message = message;
        Exception = exception;
        Data = new(capacity:0);
    }
    
    public LogEvent(
        LogLevel level, 
        [HandlesResourceDisposal] LogMessageBuilder logMessage,
        Exception? exception = null)
    {
        Timestamp = DateTime.Now;
        Level = level;
        Message = logMessage;
        Exception = exception;
        Data = new(capacity:0);
    }
    
    public LogEvent(
        LogLevel level, 
        Exception? exception,
        LogMessage? message = null)
    {
        Timestamp = DateTime.Now;
        Level = level;
        Message = message;
        Exception = exception;
        Data = new(capacity:0);
    }
    
    public LogEvent(
        LogLevel level, 
        Exception? exception,
        [HandlesResourceDisposal] LogMessageBuilder logMessage = default)
    {
        Timestamp = DateTime.Now;
        Level = level;
        Message = logMessage;
        Exception = exception;
        Data = new(capacity:0);
    }
    

    public void Add(string key, object? value)
    {
        this.Data.Add(key,value);
    }
    
    public void Add((string Key, object? Value) tuple)
    {
        this.Data.Add(tuple.Key, tuple.Value);
    }
    
    public void Add(KeyValuePair<string, object?> kvp)
    {
        this.Data.Add(kvp.Key, kvp.Value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        yield break;
    }
}