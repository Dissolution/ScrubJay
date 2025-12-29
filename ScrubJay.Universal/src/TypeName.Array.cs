using System.Text;

namespace ScrubJay.Interpolated;

partial class TypeName
{
    private static void WriteArrayType(StringBuilder builder, Type type)
    {
        Debug.Assert(type.IsArray);
        int rank = type.GetArrayRank();
        type = type.GetElementType()!;
        WriteTypeName(builder, type);
        builder.Append('[')
            .Append(',', rank - 1)
            .Append(']');
        return;
    }
}