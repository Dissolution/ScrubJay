using ScrubJay.Universal;

namespace ScrubJay.Enhancements.Exceptions;

[PublicAPI]
public class EnhancedArgumentOutOfRangeException : ArgumentOutOfRangeException, IEnhancedArgumentException
{
    public Argument Argument { get; }
    
    public string? Range { get; }

    public override string Message
    {
        get
        {
            var builder = new StringBuilder()
                .Append("Argument ")
                .RenderArgument(Argument)
                .Append(" is out of range `")
                .Append(Range)
                .Append('`');
            string? info = this.RefMessageField();
            if (!string.IsNullOrEmpty(info))
            {
                builder.Append(" -- ").Append(info);
            }
            return builder.ToString();
        }
    }

    public EnhancedArgumentOutOfRangeException(Argument argument, string? range)
        : base(paramName: argument.Name, actualValue: argument.ValueString, message: null)
    {
        Argument = argument;
        Range = range;
        this.RefMessageField() = null;
    }
    
    public EnhancedArgumentOutOfRangeException(Argument argument, string? range, string? info)
        : base(paramName: argument.Name, actualValue: argument.ValueString, message: info)
    {
        Argument = argument;
        Range = range;
        this.RefMessageField() = info;
    }
    
    public EnhancedArgumentOutOfRangeException(Argument argument, string? range, string? info, Exception? innerException)
        : base(message: info, innerException: innerException)
    {
        Argument = argument;
        Range = range;
        this.RefMessageField() = info;
        this.RefParamNameField() = argument.Name;
    }
    
    public override string ToString()
    {
        var builder = new StringBuilder()
            .RenderType(GetType())
            .Append(": ")
            .RenderArgument(Argument);
        string? info = this.RefMessageField();
        if (!string.IsNullOrEmpty(info))
        {
            builder.AppendLine()
                .Append("  Message: ")
                .Append(info);
        }

        if (!string.IsNullOrEmpty(StackTrace))
        {
            builder.AppendLine()
                .Append("  Stack Trace: ")
                .Append(StackTrace);
        }

        if (InnerException is not null)
        {
            builder.AppendLine()
                .Append("  Inner ")
                .RenderType(InnerException.GetType())
                .Append(": ")
                .Append(InnerException);
        }

        return builder.ToString();
    }
}