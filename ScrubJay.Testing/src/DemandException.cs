using System.Text;
using ScrubJay.Interpolated;

namespace ScrubJay.Testing;

[StackTraceHidden]
[PublicAPI]
public class DemandException : Exception
{
    public DemandException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
        
    }
}

public class ActualException : DemandException
{
    protected static string GetMessage(IActual actual, ref InterpolatedTextHandler info)
    {
        StringBuilder builder = new();
        builder
            .Append("Invalid Actual `")
            .Append(actual.ValueName ?? "value")
            .Append(": ")
            .AppendType(actual.ValueType)
            .Append(" = ")
            .Append(actual.ValueString)
            .Append('`');
        if (info.Length > 0)
        {
            builder.Append(": ")
                .Append(info.ToStringAndDispose());
        }

        return builder.ToString();
    }
    
    public IActual Actual { get; init; }

    public ActualException(IActual actual, ref InterpolatedTextHandler info) 
        : base(GetMessage(actual, ref info), null)
    {
        Actual = actual;
    }
}