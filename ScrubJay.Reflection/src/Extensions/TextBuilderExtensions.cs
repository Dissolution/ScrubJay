using ScrubJay.Universal;

namespace ScrubJay.Reflection.Extensions;

public static class TextBuilderExtensions
{
    
    
    public static TextBuilder RenderGenericTypes(this TextBuilder builder, params Type[]? genericTypes)
    {
        if (!genericTypes.IsNullOrEmpty())
        {
            builder.Append('<')
                .Delimit(", ", genericTypes, static (tb, type) => tb.Write(TypeName.For(type)))
                .Append('>');
        }

        return builder;
    }
}