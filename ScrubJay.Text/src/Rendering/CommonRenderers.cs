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
    public static void Render1DArrayTo<T>(T[] array, TextBuilder builder)
    {
        builder.Append('[')
            .Delimit(", ", array, TB.Render)
            .Append(']');
    }
    
    [RenderToMethod]
    public static void Render2DArrayTo<T>(T[,] array, TextBuilder builder)
    {
        // = new int[1024, 768]; = 1024 wide = 1024 columns
        
        int colCount = array.GetLength(0);
        int rowCount = array.GetLength(1);

        builder.Append('[')
            .Delimit(TB.NewLine, Enumerable.Range(0, rowCount), (rb, row) => rb
                .Append('[')
                .Delimit(", ", Enumerable.Range(0, colCount), (cb, col) => cb.Render(array[col, row]))
                .Append(']'))
            .Append(']');
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