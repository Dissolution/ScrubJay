namespace ScrubJay.Enums.SourceGen.Coding;

partial class CodeBuilder
{
    public CodeBuilder Delimit<T>(scoped ReadOnlySpan<char> delimiter, T[]? items, CodeBuilderValueAction<T>? buildItem)
    {
        if (items is not null && buildItem is not null)
        {
            int count = items.Length;
            if (count > 0)
            {
                buildItem(this, items[0]);
                for (var i = 1; i < count; i++)
                {
                    Write(delimiter);
                    buildItem(this, items[i]);
                }
            }
        }
        return this;
    }
}