using ScrubJay.Errors.Arguments;

namespace ScrubJay.Debugging.Logging;

[PublicAPI]
public sealed class LogMessage
{
    public static implicit operator LogMessage(
        [HandlesResourceDisposal] LogMessageBuilder interpolatedMessage) => new(interpolatedMessage);

    public CompositeFormat Template { get; init; }

    public List<Argument> Arguments { get; init; }

    public LogMessage()
    {
    }

    public LogMessage(string? message)
    {
        Template = CompositeFormat.Parse(message ?? "");
        Arguments = [];
    }

    public LogMessage([HandlesResourceDisposal] LogMessageBuilder interpolatedMessage)
    {
        // deconstruction handles disposal
        (Template, Arguments) = interpolatedMessage;
    }

    public override string ToString()
    {
        object?[] argObjects = Arguments.Select(arg => arg.Value).ToArray();
        return string.Format(null, Template, argObjects);
    }
}