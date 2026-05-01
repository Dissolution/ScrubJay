namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class ExceptionRenderer
{
    [RenderToMethod]
    public static void RenderExceptionTo(Exception exception, TextBuilder builder)
    {
        builder
            .Append($"{exception:@T}:")
            .Indented(propBuilder => propBuilder
                .NewLine()
                .AppendLine($"Message: \"{exception.Message}\"")
                .AppendLine($"HResult: 0x{exception.HResult:X8}")
                .IfNotEmpty(exception.HelpLink, static (tb, hl) => tb.AppendLine($"HelpLink: {hl}"))
                .If(exception.Data, static data => data.Count > 0,
                    static (tb, data) => tb
                        .Append($"Data x{data.Count}")
                        .Indented(t => t
                            .NewLine()
                            .Enumerate(data.OfType<DictionaryEntry>(),
                                static (tb, entry) => tb.AppendLine($"{entry.Key:@}: {entry.Value:@}"))))
                .IfNotEmpty(exception.Source,
                    static (tb, source) => tb.AppendLine($"Source: \"{source}\""))
                .IfNotNull(exception.TargetSite,
                    static (tb, ts) => tb.Append("Target Site: ").Render(ts).NewLine())
                .IfNotEmpty(exception.StackTrace,
                    static (tb, st) => tb
                        .Append("StackTrace:")
                        .Indented(ib => ib.NewLine().Append(st))
                        .NewLine())
                .IfNotNull(exception.InnerException,
                    (tb, inner) => tb
                        .Append("Inner Exception:")
                        .Indented(ib => ib.NewLine().Render(inner))
                        .NewLine())
                .If(exception.Is<AggregateException>(),
                    (tb, agg) => tb
                        .Append("Inner Exceptions:")
                        .Indented(ib => ib
                            .Enumerate(agg.InnerExceptions, static (ab, ex) => ab.NewLine().Render(ex)))));
    }
}