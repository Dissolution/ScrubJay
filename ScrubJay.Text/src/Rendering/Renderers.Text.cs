namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static partial class Renderers
{
    [RenderToMethod]
    public static void RenderCharTo(char ch, TextBuilder builder)
    {
        builder.Append('\'').Append(ch).Append('\'');
    }

    [RenderToMethod]
    public static void RenderStringTo(string? str, TextBuilder builder)
    {
        if (str is null)
        {
            builder.Append("null");
        }
        else
        {
            builder.Append('"').Append(str).Append('"');
        }
    }

    [RenderToMethod]
    public static void RenderTextTo(scoped text text, TextBuilder builder)
    {
        builder.Append('"').Append(text).Append('"');
    }
}