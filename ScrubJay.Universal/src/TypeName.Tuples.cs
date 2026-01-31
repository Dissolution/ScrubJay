namespace ScrubJay.Universal;

partial class TypeName
{
    internal static void WriteTuple(
        StringBuilder builder, 
        Type type,
        Type[]? genericTypes = null,
        bool appendParens = true)
    {
        if (appendParens)
        {
            builder.Append('(');
        }

        genericTypes ??= type.GetGenericArguments();

        if (genericTypes.Length > 0)
        {
            Type gt = genericTypes[0];
            checkedAppend(builder, gt);
            for (int i = 1; i < genericTypes.Length; i++)
            {
                builder.Append(", ");
                checkedAppend(builder, genericTypes[i]);
            }
        }
        
        if (appendParens)
        {
            builder.Append(')');
        }

        return;

        static void checkedAppend(StringBuilder sb, Type t)
        {
            if (!t.IsTuple)
            {
                sb.AppendTypeName(t);
            }
            else
            {
                WriteTuple(sb, t, null, false);
            }
        }
    }
}