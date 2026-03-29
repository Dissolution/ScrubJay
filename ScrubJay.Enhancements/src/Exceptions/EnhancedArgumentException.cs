using ScrubJay.Universal;

namespace ScrubJay.Enhancements.Exceptions;

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
                .RenderArgument(Argument);
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
        Argument = argument;
    }

    public EnhancedArgumentException(Argument argument, string? info)
        : base(message: info, paramName: argument.Name)
    {
        Argument = argument;
    }

    public EnhancedArgumentException(Argument argument, string? info, Exception? innerException)
        : base(message: info, paramName: argument.Name, innerException: innerException)
    {
        Argument = argument;
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