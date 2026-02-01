#nullable enable

namespace ScrubJay.Validation;

partial class Validate
{
    public static Result<int> InsertIndex(int index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index <= (uint)available)
            return index;
        return Ex.Index(index, available, $"not valid for insertion in [{available}]");
    }
    
    public static Result<int> Index(int index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        if ((uint)index < (uint)available)
            return index;
        return Ex.Index(index, available, $"not valid for indexing in [{available}]");
    }
    
    public static Result<int> InsertIndex(Index index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        int offset = index.GetOffset(available);
        if ((uint)offset <= (uint)available)
            return offset;
        return Ex.Index(index, available, $"not valid for insertion in [{available}]");
    }
    
    public static Result<int> Index(Index index, int available,
        [CallerArgumentExpression(nameof(index))]
        string? indexName = null)
    {
        int offset = index.GetOffset(available);
        if ((uint)offset < (uint)available)
            return offset;
        return Ex.Index(index, available, $"not valid for indexing in [{available}]");
    }
}
