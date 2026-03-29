using ScrubJay.Universal;

namespace ScrubJay.Enhancements.Exceptions;

[PublicAPI]
public class EnhancedArgumentNullException : ArgumentNullException, IEnhancedArgumentException
{
    public Argument Argument { get; }

    public override string Message
    {
        get
        {
            var builder = new StringBuilder()
                .Append("Null argument");

            if (Argument.IsNull)
                return builder.ToString();

            builder.Append(": ");

            if (Argument.Name is not null)
            {
                builder.Append($"\"{Argument.Name}\"");

                if (Argument.Type is not null)
                {
                    builder.Append(' ');
                }
            }

            if (Argument.Type is not null)
            {
                builder
                    .Append('(')
                    .RenderType(Argument.Type)
                    .Append(')');

            }

            string? info = this.RefMessageField();
            if (!string.IsNullOrEmpty(info))
            {
                builder.Append(" -- ").Append(info);
            }
            return builder.ToString();
        }
    }

    public EnhancedArgumentNullException(Argument argument)
        : base(paramName: argument.Name)
    {
        Argument = argument;
    }

    public EnhancedArgumentNullException(Argument argument, string? info)
        : base(paramName: argument.Name, message: info)
    {
        Argument = argument;
        this.RefMessageField() = info;
    }

    public EnhancedArgumentNullException(Argument argument, string? info, Exception? innerException)
        : base(message: info, innerException: innerException)
    {
        Argument = argument;
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