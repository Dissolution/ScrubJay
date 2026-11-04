using ScrubJay.Reflection.Lightweight.Extensions;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class TypeAlias
{
#if NET8_0_OR_GREATER
    private static readonly FrozenDictionary<Type, string> _cSharpSystemTypeAliases
#else
    private static readonly Dictionary<Type, string> _cSharpSystemTypeAliases
#endif
            = new Dictionary<Type, string>
                {
                    [typeof(byte)] = "byte",
                    [typeof(sbyte)] = "sbyte",
                    [typeof(short)] = "short",
                    [typeof(ushort)] = "ushort",
                    [typeof(int)] = "int",
                    [typeof(uint)] = "uint",
                    [typeof(long)] = "long",
                    [typeof(ulong)] = "ulong",
                    [typeof(nint)] = "nint",
                    [typeof(nuint)] = "nuint",
                    [typeof(float)] = "float",
                    [typeof(double)] = "double",
                    [typeof(decimal)] = "decimal",
                    [typeof(bool)] = "bool",
                    [typeof(char)] = "char",
                    [typeof(string)] = "string",
                    [typeof(object)] = "object",
                    [typeof(void)] = "void",
                }
#if NET8_0_OR_GREATER
                .ToFrozenDictionary()
#endif
        ;


    extension(ref DefaultInterpolatedStringHandler aliasBuilder)
    {
        private void WriteArrayTypeAlias(Type arrayType)
        {
            Debug.Assert(arrayType.IsArray);

            Type? elementType = arrayType.GetElementType();
            Debug.Assert(elementType is not null);

            // non-nested array is just the rank count in commas
            if (!elementType!.IsArray)
            {
                aliasBuilder.WriteTypeAlias(elementType);
                aliasBuilder.AppendLiteral("[");
                aliasBuilder.AppendLiteral(new string(',', arrayType.GetArrayRank() - 1));
                aliasBuilder.AppendLiteral("]");
                return;
            }

            // we need to print the root element type, then the array ranks in order from outmost to inmost
            Queue<int> ranks = new();
            ranks.Enqueue(arrayType.GetArrayRank());

            while (elementType!.IsArray)
            {
                ranks.Enqueue(elementType.GetArrayRank());
                var subElementType = elementType.GetElementType();
                if (subElementType is null)
                    break;
                elementType = subElementType;
            }

            aliasBuilder.WriteTypeAlias(elementType);
            foreach (int rank in ranks)
            {
                aliasBuilder.AppendFormatted("[");
                aliasBuilder.AppendLiteral(new string(',', rank - 1));
                aliasBuilder.AppendFormatted("]");
            }
        }

        private void WriteNestedTypeAlias(Type type,
            Type parent,
            Type[] genericTypes,
            ref int offset)
        {
            if (parent.IsGenericType)
            {
                aliasBuilder.WriteNestedTypeAlias(parent, parent.ParentType!, genericTypes, ref offset);
            }
            else
            {
                aliasBuilder.AppendLiteral(parent.Name);
            }

            aliasBuilder.AppendLiteral(".");

            if (type.IsGenericType)
            {
                int count = type.GetGenericArguments().Length;
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
                aliasBuilder.WriteNameAndGenericTypeAliases(type, slice);
            }
            else
            {
                aliasBuilder.AppendLiteral(type.Name);
            }
        }

        private void WriteNameAndGenericTypeAliases(Type type,
            scoped ReadOnlySpan<Type> genericTypes)
        {
            string name = type.Name;
            int i = name.LastIndexOf('`', StringComparison.Ordinal);
            if (i >= 0)
            {
                aliasBuilder.AppendFormatted(name.AsSpan(0, i));
            }
            else
            {
                aliasBuilder.AppendLiteral(name);
            }

            string sep = type.IsGenericTypeDefinition ? "," : ", ";

            if (genericTypes.Length > 0)
            {
                aliasBuilder.AppendLiteral("<");
                aliasBuilder.WriteTypeAlias(genericTypes[0]);
                for (i = 1; i < genericTypes.Length; i++)
                {
                    aliasBuilder.AppendLiteral(sep);
                    aliasBuilder.WriteTypeAlias(genericTypes[i]);
                }
                aliasBuilder.AppendLiteral(">");
            }
        }

        private void WriteTupleTypeAlias(Type type,
            Type[]? genericTypes,
            bool appendParens = true)
        {
            if (appendParens)
            {
                aliasBuilder.AppendLiteral("(");
            }

            genericTypes ??= type.GetGenericArguments();

            if (genericTypes.Length > 0)
            {
                writeTupleItemTypeAlias(ref aliasBuilder, genericTypes[0]);
                for (int i = 1; i < genericTypes.Length; i++)
                {
                    aliasBuilder.AppendLiteral(", ");
                    writeTupleItemTypeAlias(ref aliasBuilder, genericTypes[i]);
                }
            }

            if (appendParens)
            {
                aliasBuilder.AppendLiteral(")");
            }

            return;

            static void writeTupleItemTypeAlias(ref DefaultInterpolatedStringHandler ab, Type itemType)
            {
                if (!itemType.IsTuple)
                {
                    ab.WriteTypeAlias(itemType);
                }
                else
                {
                    ab.WriteTupleTypeAlias(itemType, null, false);
                }
            }
        }

        private void WriteTypeAlias(Type? type)
        {
            if (type is null)
            {
                aliasBuilder.AppendLiteral("typeof(null)");
                return;
            }

            if (_cSharpSystemTypeAliases.TryGetValue(type, out var alias))
            {
                aliasBuilder.AppendLiteral(alias);
                return;
            }

            if (type.IsPointer)
            {
                Debug.Assert(type.GetElementType() is not null);
                aliasBuilder.WriteTypeAlias(type.GetElementType());
                aliasBuilder.AppendLiteral("*");
                return;
            }

            if (type.IsByRef)
            {
                Debug.Assert(type.GetElementType() is not null);
                aliasBuilder.WriteTypeAlias(type.GetElementType());
                aliasBuilder.AppendLiteral("&");
                return;
            }

            if (type.IsArray)
            {
                aliasBuilder.WriteArrayTypeAlias(type);
                return;
            }

            Type[] genericTypes = type.GetGenericArguments();

            if (type is { IsNested: true, IsGenericParameter: false })
            {
                Type parent = type.ParentType!;
                if (parent.IsGenericType)
                {
                    int offset = 0;
                    aliasBuilder.WriteNestedTypeAlias(type, parent, genericTypes, ref offset);
                    return;
                }

                aliasBuilder.WriteTypeAlias(parent);
                aliasBuilder.AppendLiteral(".");
            }

            if (type.IsGenericType)
            {
                if (type.IsTuple)
                {
                    aliasBuilder.WriteTupleTypeAlias(type, genericTypes);
                    return;
                }

                Type? underType = Nullable.GetUnderlyingType(type);
                if (underType is not null)
                {
                    Debug.Assert(genericTypes.Length == 1);
                    aliasBuilder.WriteTypeAlias(genericTypes[0]);
                    aliasBuilder.AppendLiteral("?");
                    return;
                }

                aliasBuilder.WriteNameAndGenericTypeAliases(type, genericTypes);
                return;
            }

            aliasBuilder.AppendLiteral(type.Name);
        }
    }

    [return: NotNullIfNotNull(nameof(type))]
    public static string? For(Type? type)
    {
        if (type is null)
            return null;

        DefaultInterpolatedStringHandler nameBuilder = new(64, 0);
        nameBuilder.WriteTypeAlias(type);
        return nameBuilder.ToStringAndClear();
    }

    public static string For<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => For(typeof(T));

    public static string For<I>(I? instance) => For(instance?.GetType() ?? typeof(I));

#if NET9_0_OR_GREATER
    public static string For<I>(in I? instance)
        where I : allows ref struct
        => For(Generic.GetType<I>(in instance));
#endif
}