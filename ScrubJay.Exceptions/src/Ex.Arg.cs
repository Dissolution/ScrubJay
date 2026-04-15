using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

public partial class Ex
{

}

internal sealed class SJArgumentException : ArgumentException
{
    private static string GetMessage(Argument argument, string? info = null)
    {
        var text = InterpolatedText.Create($"Invalid Argument '{argument.Name}'");

        if (argument.Type is not null || argument.ValueString is not null)
        {
            if (argument.Type is not null)
            {
                text.AppendLiteral(" (");
                text.RenderType(argument.Type);
                text.AppendLiteral(')');
            }

            if (argument.ValueString is not null)
            {
                text.AppendLiteral(" = ");
                text.AppendLiteral(argument.ValueString);
            }
        }
        else
        {
            text.AppendLiteral(" (null)");
        }

        if (!string.IsNullOrEmpty(info))
        {
            text.AppendLiteral(": ");
            text.AppendLiteral(info!);
        }

        return text.ToStringAndDispose();
    }

    public Argument Argument { get; }
    
    public override string Message
    {
        get
        {
            // we don't want to use what they put, only what we stored in _message
            ref string? message = ref ExFields.RefMessageField(this);
            if (message is null)
                message = GetMessage(Argument);
            return message;
        }
    }

    public SJArgumentException(Argument argument, string? info = null, Exception? innerException = null)
        : base(GetMessage(argument, info), innerException)
    {
        this.Argument = argument;
        ExFields.RefParamNameField(this) = argument.Name;
        ExFields.RefInnerExceptionField(this) = innerException;
    }
}