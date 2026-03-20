namespace ScrubJay.Exceptions;

[PublicAPI]
public class EnhancedArgumentException : ArgumentException, IEnhancedArgumentException
{
    protected static string GetMessage(Argument argument, string? info = null)
    {
        var builder = new StringBuilder();
        builder.RenderArgument(argument)
            .Append(" was invalid");
        if (!string.IsNullOrEmpty(info))
        {
            builder.Append(": ").Append(info);
        }
        return builder.ToString();
    }

    protected static string ToString(EnhancedArgumentException ex)
    {
        return ex.ToString();
    }

    public Argument Argument { get; }

    public override string Message => this.RefMessageField()!;

    public EnhancedArgumentException(Argument argument)
        : base(GetMessage(argument), argument.Name)
    {
        this.Argument = argument;
    }
    
    public EnhancedArgumentException(Argument argument, string? info)
        : base(GetMessage(argument, info), argument.Name)
    {
        this.Argument = argument;
    }
    
    public EnhancedArgumentException(Argument argument, string? info, Exception? innerException)
        : base(GetMessage(argument, info), argument.Name, innerException)
    {
        this.Argument = argument;
    }

    public override string ToString() => ToString(this);
}