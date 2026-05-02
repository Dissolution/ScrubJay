
//

//
//
//        public void WriteTo(TextBuilder text, int offset = 0)
//        {
//            text.Render(typeof(E));
//            text.Append(':');
//            offset++;
//
//            var message = exception.Message;
//            if (!string.IsNullOrEmpty(message))
//            {
//                text.AppendLine();
//                text.Repeat(offset, "  ");
//                text.Append($"Message: {message}");
//            }
//            
//            exception.WriteDebugInformationTo(text, offset);
//            exception.WriteOptionalPropertiesTo(text, offset);
//            offset--;
//        }
//    }
//}


namespace ScrubJay.Exceptions;

internal static class WeirdExtensions
{
    public static TextBuilder AppendLineIfNotNull<T>(
        this TextBuilder builder, 
        T? value,
        [InterpolatedStringHandlerArgument(nameof(builder))]
        ref InterpolatedTextBuilder interpolatedText)
    {
        if (value is not null)
        {
            return builder
                .NewLine()
                .Append(ref interpolatedText);
        }
        return builder;
    }
    
    public static TextBuilder AppendLineIfNotEmpty(
        this TextBuilder builder, 
        string? str,
        [InterpolatedStringHandlerArgument(nameof(builder))]
        ref InterpolatedTextBuilder interpolatedText)
    {
        if (!string.IsNullOrEmpty(str))
        {
            return builder
                .NewLine()
                .Append(ref interpolatedText);
        }
        return builder;
    }

}