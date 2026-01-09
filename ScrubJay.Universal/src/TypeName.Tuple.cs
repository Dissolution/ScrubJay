namespace ScrubJay.Universal;

partial class TypeName
{
    private static void WriteTupleType(StringBuilder builder, Type type, GenericTypes genericTypes)
    {
        builder.Append('(');
        if (genericTypes.TryGet(out var gt))
        {
            WriteType(builder, gt, genericTypes);
        }
        
        while (genericTypes.TryGet(out gt))
        {
            builder.Append(", ");
            WriteType(builder, gt, genericTypes);
        }
       
        builder.Append(')');
    }
}