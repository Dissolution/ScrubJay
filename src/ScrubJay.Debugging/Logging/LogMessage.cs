namespace ScrubJay.Debugging.Logging;

[PublicAPI]
public sealed class LogMessage
{
    public static implicit operator LogMessage(
        [HandlesResourceDisposal] LogMessageBuilder interpolatedMessage) => new(interpolatedMessage);
    
    public string? Template { get; init; }
    
    public LogMessageArguments Arguments { get; init; } = [];

    public LogMessage()
    {
        
    }

    public LogMessage(string? message)
    {
        Template = message;
    }

    public LogMessage([HandlesResourceDisposal] LogMessageBuilder interpolatedMessage)
    {
        // deconstruction handles disposal
        (Template, Arguments) = interpolatedMessage;
    }
    
    public override string ToString()
    {
        // todo
        return $"{Template}: {Arguments}";
    }
}