using System.Text;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Functional.Utilities;

partial class TypeName
{
#if NET8_0_OR_GREATER
    private static readonly FrozenSet<Type> _tupleTypeDefinitions =
#else
    private static readonly HashSet<Type> _tupleTypeDefinitions =
#endif
            new HashSet<Type>
                {
                    typeof(ValueTuple<>),
                    typeof(ValueTuple<,>),
                    typeof(ValueTuple<,,>),
                    typeof(ValueTuple<,,,>),
                    typeof(ValueTuple<,,,,>),
                    typeof(ValueTuple<,,,,,>),
                    typeof(ValueTuple<,,,,,,>),
                    typeof(ValueTuple<,,,,,,,>),
                    typeof(Tuple<>),
                    typeof(Tuple<,>),
                    typeof(Tuple<,,>),
                    typeof(Tuple<,,,>),
                    typeof(Tuple<,,,,>),
                    typeof(Tuple<,,,,,>),
                    typeof(Tuple<,,,,,,>),
                    typeof(Tuple<,,,,,,,>),
                }
#if NET8_0_OR_GREATER
                .ToFrozenSet()
#endif
        ;
  
    internal static bool IsGenericTuple(Type type, Type? genericTypeDefinition = null)
    {
        if (genericTypeDefinition is null)
        {
            if (!type.IsGenericType) 
                return false;
            genericTypeDefinition = type.GetGenericTypeDefinition();
        }

        return _tupleTypeDefinitions.Contains(genericTypeDefinition);
    }
    
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
            if (!IsGenericTuple(t))
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