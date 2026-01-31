namespace ScrubJay.Universal;

partial class TypeName
{
    internal static StringBuilder AppendArrayType(
        this StringBuilder builder,
        Type arrayType)
    {
        Debug.Assert(arrayType.IsArray);
        
        Type? elementType = arrayType.GetElementType();
        Debug.Assert(elementType is not null);
        
        // if we aren't a nested array, we can just print our ranks and return
        if (!elementType!.IsArray)
        {
            return builder
                .AppendTypeName(elementType)
                .Append('[')
                .Append(',', arrayType.GetArrayRank() - 1)
                .Append(']');
        }

        // we need to print the root element type, then the array ranks in order from outmost to inmost
        Queue<int> ranks = new();
        ranks.Enqueue(arrayType.GetArrayRank());

        while (elementType is not null && elementType.IsArray)
        {
            ranks.Enqueue(elementType.GetArrayRank());
            elementType = elementType.GetElementType();
        }

        builder.AppendTypeName(elementType);
        foreach (int rank in ranks)
        {
            builder.Append('[')
                .Append(',', rank - 1)
                .Append(']');
        }

        return builder;
    }



}