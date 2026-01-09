namespace ScrubJay.Validation;

partial class Guard
{
    public static int InsertIndex(int index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index <= (uint)available)
            return index;
        throw Ex.Index(index, available, $"not valid for insertion in [{available}]");
    }
    
    public static int Index(int index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index < (uint)available)
            return index;
        throw Ex.Index(index, available, $"not valid for indexing in [{available}]");
    }
    
    public static int InsertIndex(Index index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        int offset = index.GetOffset(available);
        if ((uint)offset <= (uint)available)
            return offset;
        throw Ex.Index(index, available, $"not valid for insertion in [{available}]");
    }
    
    public static int Index(Index index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        int offset = index.GetOffset(available);
        if ((uint)offset < (uint)available)
            return offset;
        throw Ex.Index(index, available, $"not valid for indexing in [{available}]");
    }
}