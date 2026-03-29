namespace ScrubJay.Enhancements.Exceptions;

[PublicAPI]
public static class ArgumentExtensions
{
    extension([NotNullWhen(false)] Argument? argument)
    {
        public bool IsNull
        {
            get
            {
                return argument is null || (argument.Name is null && argument.Type is null && argument.ValueString is null);
            }
        }
    }
    
    extension([NotNullWhen(true)] Argument? argument)
    {
        public bool IsNotNull
        {
            get
            {
                return argument is not null && (argument.Name is not null || argument.Type is not null || argument.ValueString is not null);
            }
        }
    }
}