using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

[PublicAPI]
public class EnhancedArgumentException : ArgumentException, IEnhancedArgumentException
{
    public Argument Argument { get; }

    public override string Message
    {
        get
        {
            var builder = new StringBuilder()
                .Append("Invalid argument: ")
                .RenderArgument(this.Argument);
            string? info = this.RefMessageField();
            if (!string.IsNullOrEmpty(info))
            {
                builder.Append(" -- ").Append(info);
            }
            return builder.ToString();
        }
    }

    public EnhancedArgumentException(Argument argument)
        : base(message: null, paramName: argument.Name)
    {
        this.Argument = argument;
    }
    
    public EnhancedArgumentException(Argument argument, string? info)
        : base(message: info, paramName: argument.Name)
    {
        this.Argument = argument;
    }
    
    public EnhancedArgumentException(Argument argument, string? info, Exception? innerException)
        : base(message: info, paramName: argument.Name, innerException: innerException)
    {
        this.Argument = argument;
    }

    public override string ToString()
    {
        var builder = new StringBuilder()
            .RenderType(GetType())
            .Append(": ")
            .RenderArgument(this.Argument);
        string? info = this.RefMessageField();
        if (!string.IsNullOrEmpty(info))
        {
            builder.AppendLine()
                .Append("  Message: ")
                .Append(info);
        }

        if (!string.IsNullOrEmpty(this.StackTrace))
        {
            builder.AppendLine()
                .Append("  Stack Trace: ")
                .Append(StackTrace);
        }

        if (this.InnerException is not null)
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