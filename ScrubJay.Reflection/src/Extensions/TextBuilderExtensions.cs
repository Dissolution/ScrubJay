using ScrubJay.Text.Building;
using ScrubJay.Text.Rendering;

namespace ScrubJay.Reflection.Extensions;

public static class TextBuilderExtensions
{
    
    
    public static TextBuilder RenderGenericTypes(this TextBuilder builder, params Type[]? genericTypes)
    {
        if (!genericTypes.IsNullOrEmpty())
        {
            builder.Append('<')
                .Delimit(", ", genericTypes, TB.Render)
                .Append('>');
        }

        return builder;
    }
}