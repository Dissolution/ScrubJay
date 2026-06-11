namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class CommonRenderers
{
    // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#general-format-specifier-g

    [RenderToMethod]
    public static void RenderUInt32To(uint u32, TextBuilder builder)
    {
        builder.Append(u32).Append('U');
    }

    [RenderToMethod]
    public static void RenderInt64To(long i64, TextBuilder builder)
    {
        builder.Append(i64).Append('L');
    }

    [RenderToMethod]
    public static void RenderUInt64To(ulong u64, TextBuilder builder)
    {
        builder.Append(u64).Append("UL");
    }

    [RenderToMethod]
    public static void RenderF32To(float f32, TextBuilder builder)
    {
        builder.Format(f32, "G9").Write('f');
    }

    [RenderToMethod]
    public static void RenderF64To(double f64, TextBuilder builder)
    {
        builder.Format(f64, "G17").Write('d');
    }

    [RenderToMethod]
    public static void RenderDecimalTo(decimal dec, TextBuilder builder)
    {
        builder.Format(dec, "G").Write('m');
    }

    [RenderToMethod]
    public static void RenderTimeSpanTo(TimeSpan timeSpan, TextBuilder builder)
    {
        builder.Format(timeSpan, "g");
    }

    [RenderToMethod]
    public static void RenderDateTimeTo(DateTime dateTime, TextBuilder builder)
    {
        builder.Format(dateTime, "yyyy-MM-dd HH:mm:ss");
    }

    [RenderToMethod]
    public static void RenderDateTimeOffsetTo(DateTimeOffset dateTimeOffset, TextBuilder builder)
    {
        builder.Format(dateTimeOffset, "u");
    }

#if NET6_0_OR_GREATER
    [RenderToMethod]
    public static void RenderTimeOnlyTo(TimeOnly time, TextBuilder builder)
    {
        builder.Format(time, "T");
    }

    [RenderToMethod]
    public static void RenderDateOnlyTo(DateOnly date, TextBuilder builder)
    {
        builder.Format(date, "O");
    }
#endif



    [RenderToMethod]
    public static void RenderDBNullTo(DBNull _, TextBuilder builder)
    {
        builder.Write(nameof(DBNull));
    }

    [RenderToMethod]
    public static void RenderBooleanTo(bool boolean, TextBuilder builder)
    {
        if (boolean)
        {
            builder.Write("true");
        }
        else
        {
            builder.Write("false");
        }
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
    
    [RenderToMethod]
    public static void RenderGuidTo(Guid guid, TextBuilder builder)
    {
        builder.Format(guid, "D");
    }

    [RenderToMethod]
    public static void RenderRenderableTo<R>(R renderable, TextBuilder builder)
        where R : IRenderable
#if NET9_0_OR_GREATER
    , allows ref struct
#endif
    {
        renderable.RenderTo(builder);
    }
}