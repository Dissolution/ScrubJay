namespace ScrubJay.Exceptions;

public class NullArgEx : ArgumentNullException, IEnhancedException
{
    public NullArgEx(string? paramName) : base(paramName)
    {
    }

    public NullArgEx(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public NullArgEx(string? paramName, string? message) : base(paramName, message)
    {
    }
}