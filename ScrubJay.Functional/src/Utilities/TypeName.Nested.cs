using System.Text;

namespace ScrubJay.Functional.Utilities;

partial class TypeName
{
    private static StringBuilder AppendComplexNestedName(
        StringBuilder builder, 
        Type type, 
        Type parent,
        Type[] genericTypes)
    {
        int offset = 0;

        renderNesting(type, parent);
        return builder;

        void renderNesting(Type t, Type p)
        {
            if (p.IsGenericType)
            {
                renderNesting(p, p.ParentType!);
            }
            else
            {
                builder.Append(p.Name);
            }

            builder.Append('.');

            if (t.IsGenericType)
            {
                int count = t.GetGenericArguments().Length;
                ReadOnlySpan<Type> slice;
                if (offset + count > genericTypes.Length)
                {
                    slice = genericTypes.AsSpan(offset);
                }
                else
                {
                    slice = genericTypes.AsSpan(offset, count);
                }

                offset += count;
                AppendNameAndGenericTypes(builder, t, slice);
            }
            else
            {
                builder.Append(type.Name);
            }
        }
    }

}