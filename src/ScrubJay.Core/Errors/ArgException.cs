namespace ScrubJay.Errors;

public class ArgException : ArgumentException
{
    public ArgumentInfo ArgumentInfo { get; }

    public override string Message { get; }

    public ArgException(
        ArgumentInfo argumentInfo, 
        string? message = null, 
        Exception? innerException = null)
        : base(message, argumentInfo.Name, innerException)
    {
        ArgumentInfo = argumentInfo;
    }
}