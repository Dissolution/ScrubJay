using System.Text;

namespace ScrubJay.Functional.Utilities;

partial class TypeName
{
    internal static StringBuilder AppendNameAndGenericTypes(
        StringBuilder builder,
        Type type,
        params ReadOnlySpan<Type> genericTypes)
    {
        int i = type.Name.LastIndexOf('`');
        if (i >= 0)
        {
            builder.Append(type.Name, 0, i);
        }
        else
        {
            builder.Append(type.Name);
        }

        string sep = type.IsGenericTypeDefinition ? "," : ", ";

        if (genericTypes.Length > 0)
        {
            builder.Append('<').AppendTypeName(genericTypes[0]);
            for (i = 1; i < genericTypes.Length; i++)
            {
                builder.Append(sep).AppendTypeName(genericTypes[i]);
            }

            builder.Append('>');
        }

        return builder;
    }
}