namespace ScrubJay.Rendering.Rendition3;

[PublicAPI]
public static class RenderingExtensions
{
    [Renders<Guid>]
    public static TextBuilder RenderTo(this Guid guid, TextBuilder builder)
    {
        var buffer = builder.Allocate(36);
#if DEBUG
        bool formatted = guid.TryFormat(buffer, out int charsWritten, "D");
        Debug.Assert(formatted);
        Debug.Assert(charsWritten == 36);
#else
                guid.TryFormat(buffer, out _, "D");
#endif
        buffer.ForEach((ref ch) => ch = char.ToUpper(ch));
        return builder;
    }

    [Renders<Enum>]
    public static TextBuilder RenderTo(this Enum @enum, TextBuilder builder)
    {
        if (EnumTypeInfo.For(@enum).TryGetMemberInfo(@enum).IsSome(out var info))
        {
            return info.RenderTo(builder);
        }

        return builder.Append(@enum.ToString());
    }
}