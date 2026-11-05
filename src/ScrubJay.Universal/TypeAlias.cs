
using ScrubJay.Universal.Extensions;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Universal;

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


    extension(StringBuilder aliasBuilder)
    {
        private StringBuilder AppendArrayTypeAlias(Type arrayType)
        {
            Debug.Assert(arrayType.IsArray);

            Type? elementType = arrayType.GetElementType();
            Debug.Assert(elementType is not null);

            // non-nested array is just the rank count in commas
            if (!elementType!.IsArray)
            {
                return aliasBuilder.AppendTypeAlias(elementType)
                    .Append('[')
                    .Append(',', arrayType.GetArrayRank() - 1)
                    .Append(']');
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

            aliasBuilder.AppendTypeAlias(elementType);

            foreach (int rank in ranks)
            {
                aliasBuilder.Append('[')
                    .Append(',', rank - 1)
                    .Append(']');
            }

            return aliasBuilder;
        }

        private StringBuilder AppendNestedTypeAlias(Type type,
            Type parent,
            Type[] genericTypes,
            ref int offset)
        {
            if (parent.IsGenericType)
            {
                aliasBuilder.AppendNestedTypeAlias(parent, parent.ParentType!, genericTypes, ref offset);
            }
            else
            {
                aliasBuilder.Append(parent.Name);
            }

            aliasBuilder.Append('.');

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
                aliasBuilder.AppendNameAndGenericTypeAliases(type, slice);
            }
            else
            {
                aliasBuilder.Append(type.Name);
            }

            return aliasBuilder;
        }

        private StringBuilder AppendNameAndGenericTypeAliases(
            Type type,
            scoped ReadOnlySpan<Type> genericTypes)
        {
            string name = type.Name;
            int i = name.LastIndexOf('`');
            if (i >= 0)
            {
#if NETSTANDARD2_0
                aliasBuilder.Append(name.Substring(0, i));
#else
                aliasBuilder.Append(name.AsSpan(0, i));
#endif
            }
            else
            {
                aliasBuilder.Append(name);
            }

            string sep = type.IsGenericTypeDefinition ? "," : ", ";

            if (genericTypes.Length > 0)
            {
                aliasBuilder.Append('<');
                aliasBuilder.AppendTypeAlias(genericTypes[0]);
                for (i = 1; i < genericTypes.Length; i++)
                {
                    aliasBuilder.Append(sep);
                    aliasBuilder.AppendTypeAlias(genericTypes[i]);
                }
                aliasBuilder.Append('>');
            }

            return aliasBuilder;
        }

        private StringBuilder AppendTupleTypeAlias(
            Type type,
            Type[]? genericTypes,
            bool appendParens = true)
        {
            if (appendParens)
            {
                aliasBuilder.Append('(');
            }

            genericTypes ??= type.GetGenericArguments();

            if (genericTypes.Length > 0)
            {
                writeTupleItemTypeAlias(aliasBuilder, genericTypes[0]);
                for (int i = 1; i < genericTypes.Length; i++)
                {
                    aliasBuilder.Append(", ");
                    writeTupleItemTypeAlias(aliasBuilder, genericTypes[i]);
                }
            }

            if (appendParens)
            {
                aliasBuilder.Append(')');
            }

            return aliasBuilder;

            static void writeTupleItemTypeAlias(StringBuilder ab, Type itemType)
            {
                if (!itemType.IsTuple)
                {
                    ab.AppendTypeAlias(itemType);
                }
                else
                {
                    ab.AppendTupleTypeAlias(itemType, null, false);
                }
            }
        }

        private StringBuilder AppendTypeAlias(Type? type)
        {
            if (type is null)
            {
                return aliasBuilder.Append("typeof(null)");
            }

            if (_cSharpSystemTypeAliases.TryGetValue(type, out var alias))
            {
                return aliasBuilder.Append(alias);
            }

            if (type.IsPointer)
            {
                Debug.Assert(type.GetElementType() is not null);
                return aliasBuilder.AppendTypeAlias(type.GetElementType())
                    .Append('*');
            }

            if (type.IsByRef)
            {
                Debug.Assert(type.GetElementType() is not null);
                return aliasBuilder.AppendTypeAlias(type.GetElementType())
                    .Append('&');
            }

            if (type.IsArray)
            {
                return aliasBuilder.AppendArrayTypeAlias(type);
            }

            Type[] genericTypes = type.GetGenericArguments();

            if (type is { IsNested: true, IsGenericParameter: false })
            {
                Type parent = type.ParentType!;
                if (parent.IsGenericType)
                {
                    int offset = 0;
                    return aliasBuilder.AppendNestedTypeAlias(type, parent, genericTypes, ref offset);
                }

                aliasBuilder.AppendTypeAlias(parent)
                    .Append('.');
            }

            if (type.IsGenericType)
            {
                if (type.IsTuple)
                {
                    return aliasBuilder.AppendTupleTypeAlias(type, genericTypes);
                }

                Type? underType = Nullable.GetUnderlyingType(type);
                if (underType is not null)
                {
                    Debug.Assert(genericTypes.Length == 1);
                    return aliasBuilder.AppendTypeAlias(genericTypes[0])
                        .Append('?');
                }

                return aliasBuilder.AppendNameAndGenericTypeAliases(type, genericTypes);
            }

            return aliasBuilder.Append(type.Name);
        }
    }

    [return: NotNullIfNotNull(nameof(type))]
    public static string? For(Type? type)
    {
        if (type is null)
            return null;

        return new StringBuilder().AppendTypeAlias(type).ToString();
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
        => For(Anything.GetType<I>(in instance));
#endif
}