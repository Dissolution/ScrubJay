using System.Text;

namespace ScrubJay.Validation;

[StackTraceHidden]
[PublicAPI]
public class DemandException : Exception
{
    public Arg Argument { get; init; }

    public DemandException(Arg argument, string? message = null)
        : base(message)
    {
        this.Argument = argument;
    }
}

[StackTraceHidden]
[PublicAPI]
public class DemandException<T> : Exception
{
    public Arg<T> Argument { get; init; }

    public DemandException(Arg<T> argument, string? message = null)
        : base(message)
    {
        this.Argument = argument;
    }
}