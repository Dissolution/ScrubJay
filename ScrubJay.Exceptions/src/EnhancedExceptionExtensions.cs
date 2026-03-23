using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

public static class EnhancedExceptionExtensions
{
    extension<E>(E ex)
        where E : Exception
    {
        public Uri HResultInfo => new HResult(ex.HResult).InfoUri;

        internal ref string? RefMessageField()
        {
            return ref RefExceptionMessageField(ex);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    internal extern static ref string? RefExceptionMessageField(Exception exception);

    internal static StringBuilder RenderArgument(this StringBuilder builder, Argument argument)
    {
        if (argument.IsNull)
        {
            return builder.Append("null");
        }
        
        if (argument.Name is not null)
        {
            builder.Append($"\"{argument.Name}\"");
            
            if (argument.Type is not null)
            {
                builder.Append(' ');
            }
            else if (argument.ValueString is not null)
            {
                builder.Append(" = ");
            }
            else
            {
                return builder.Append(" = null");
            }
        }

        if (argument.Type is not null)
        {
            builder
                .Append('(')
                .RenderType(argument.Type)
                .Append(')');
            
            if (argument.ValueString is not null)
            {
                builder.Append(" = ");
            }
        }
        
        if (argument.ValueString is not null)
        {
            builder.Append($"`{argument.ValueString}`");
        }
        else
        {
            builder.Append("null");
        }

        return builder;
    }
}