using System.Text;

namespace ScrubJay.Text;

partial class TypeName
{
    private static StringBuilder AppendArrayType(
        StringBuilder builder,
        Type type)
    {
        Debug.Assert(type.IsArray);
        
        int rank = type.GetArrayRank();
        var elementType = type.GetElementType()!;
        if (elementType.IsArray)
        {
            // complex nesting
            Stack<int> ranks = new();
        }
        else
        {
            builder.AppendType(elementType);
            appendRank(builder, rank);
        }
        return builder;

        static StringBuilder appendRank(StringBuilder builder, int rank)
        {
            return builder
                .Append('[')
                .Append(',', repeatCount: rank - 1)
                .Append(']');
        }
    }
}