namespace ScrubJay.Exceptions;

public class OutOfRangeArgEx : ArgumentOutOfRangeException, IEnhancedException
{
    public override object? ActualValue { get; }

    public OutOfRangeArgEx(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public OutOfRangeArgEx(string? paramName, object? actualValue, string? message) : base(paramName, actualValue, message)
    {
    }
}