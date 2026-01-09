namespace ScrubJay.Universal;

partial class TypeName
{
    private static void WriteArrayType(StringBuilder builder, Type type, GenericTypes providedGenericTypes)
    {
        Debug.Assert(type.IsArray);
        var elementType = type.GetElementType()!;
        
        if (elementType.IsArray)
        {
            //nested arrays are special, the need to be printed in the opposite order
            WriteNestedArray(builder, type, providedGenericTypes);
            return;
        }
        else
        {
            WriteType(builder, elementType, providedGenericTypes);
        }
        
        int rank = type.GetArrayRank();
        builder.Append('[')
            .Append(',', rank - 1)
            .Append(']');
        
    }

    private static void WriteNestedArray(StringBuilder builder, Type type, GenericTypes providedGenericTypes)
    {
        Debug.Assert(type.IsArray);
        
        Queue<int> ranks = new();
        
        ranks.Enqueue(type.GetArrayRank());

        Type? elementType = type.GetElementType();
        while (elementType is not null && elementType.IsArray)
        {
            ranks.Enqueue(elementType.GetArrayRank());
            elementType = elementType.GetElementType();
        }

        WriteType(builder, elementType, providedGenericTypes);
        foreach (int rank in ranks)
        {
            builder.Append('[')
                .Append(',', rank - 1)
                .Append(']');
        }
    }
}