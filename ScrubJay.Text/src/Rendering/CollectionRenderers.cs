namespace ScrubJay.Text.Rendering;

[PublicAPI]
public static class CollectionRenderers
{
    [RenderToMethod(AcceptsNull = true)]
    public static void RenderDictionaryTo<D, K, V>(D? dictionary, TextBuilder builder)
        where D : IDictionary<K, V>
    {
        builder.Append('(')
            .Render(Any.GetType(in dictionary))
            .Append(')');

        if (dictionary is null)
        {
            builder.Write("null");
            return;
        }

        int count = dictionary.Count;

        builder.Append('[').Append(count).Append(']');

        if (count == 0)
            return;

        builder.Append(':').Indent().NewLine()
            .Delimit(TB.NewLine, dictionary,
                static (tb, entry) => tb.Render(entry.Key).Append(": ").Render(entry.Value))
            .Outdent();
    }

    [RenderToMethod]
    public static void RenderListTo<L, T>(L list, TextBuilder builder)
        where L : IList<T>
    {
        builder
            .RenderTypeOf<L>(list)
            .Append('(')
            .Append(list.Count)
            .Append(")[")
            .Delimit(", ", list, TB.Render)
            .Append(']');
    }
}