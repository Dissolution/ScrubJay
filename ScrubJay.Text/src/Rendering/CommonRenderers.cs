namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class CommonRenderers
{
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
    

    [RenderToMethod]
    public static void RenderTupleTo<T>(T? tuple, TextBuilder builder)
        where T : ITuple
    {
        if (tuple is not null)
        {
            builder.Append('(')
                .Delimit(", ", tuple.GetIterator(), TB.Render)
                .Append(')');
        }
    }
}