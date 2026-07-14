#if NET8_0_OR_GREATER
using System.ComponentModel;
using System.Collections.Frozen;
#endif

namespace ScrubJay.Reflection.Lightweight;

[PublicAPI]
public static class TypeName
{
#if NET8_0_OR_GREATER
    private static readonly FrozenDictionary<Type, string> _cSharpSystemTypeAliases;
    private static readonly FrozenDictionary<Type, string> _fSharpSystemTypeAliases;
    private static readonly FrozenDictionary<Type, string> _vbSystemTypeAliases;
#else
    private static readonly Dictionary<Type, string> _cSharpSystemTypeAliases;
    private static readonly Dictionary<Type, string> _fSharpSystemTypeAliases;
    private static readonly Dictionary<Type, string> _vbSystemTypeAliases;
#endif

    static TypeName()
    {
        _cSharpSystemTypeAliases = new Dictionary<Type, string>
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

        _fSharpSystemTypeAliases = new Dictionary<Type, string>
                {
                    [typeof(bool)] = "bool",
                    [typeof(byte)] = "byte",
                    [typeof(sbyte)] = "sbyte",
                    [typeof(short)] = "int16",
                    [typeof(ushort)] = "uint16",
                    [typeof(int)] = "int32",
                    [typeof(uint)] = "uint32",
                    [typeof(long)] = "int64",
                    [typeof(ulong)] = "uint64",
                    [typeof(nint)] = "nativeint",
                    [typeof(nuint)] = "unativeint",
                    [typeof(float)] = "single",
                    [typeof(double)] = "double",
                    [typeof(decimal)] = "decimal",
                    [typeof(char)] = "char",
                    [typeof(string)] = "string",
                    [typeof(object)] = "obj",
                }
#if NET8_0_OR_GREATER
                .ToFrozenDictionary()
#endif
            ;

        _vbSystemTypeAliases = new Dictionary<Type, string>
                {
                    [typeof(bool)] = "Boolean",
                    [typeof(byte)] = "Byte",
                    [typeof(sbyte)] = "SByte",
                    [typeof(short)] = "Short",
                    [typeof(ushort)] = "UShort",
                    [typeof(int)] = "Integer",
                    [typeof(uint)] = "UInteger",
                    [typeof(long)] = "Long",
                    [typeof(ulong)] = "ULong",
                    [typeof(float)] = "Single",
                    [typeof(double)] = "Double",
                    [typeof(decimal)] = "Decimal",
                    [typeof(char)] = "Char",
                    [typeof(string)] = "String",
                    [typeof(object)] = "Object",
                    [typeof(DateTime)] = "Date",
                }
#if NET8_0_OR_GREATER
                .ToFrozenDictionary()
#endif
            ;
    }

    private static void WriteArrayTypeNameTo(Type arrayType, ref DefaultInterpolatedStringHandler nameBuilder)
    {
        Debug.Assert(arrayType.IsArray);

        Type? elementType = arrayType.GetElementType();
        Debug.Assert(elementType is not null);

        // non-nested array can just append the ranks
        if (!elementType!.IsArray)
        {
            WriteTypeNameTo(elementType, ref nameBuilder);
            nameBuilder.AppendLiteral("[");
            nameBuilder.AppendLiteral(new string(',', arrayType.GetArrayRank() - 1));
            nameBuilder.AppendLiteral("]");
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

        WriteTypeNameTo(elementType, ref nameBuilder);
        foreach (int rank in ranks)
        {
            nameBuilder.AppendFormatted("[");
            nameBuilder.AppendLiteral(new string(',', rank - 1));
            nameBuilder.AppendFormatted("]");
        }
    }

    private static void WriteNestedTypeNameTo(
        Type type,
        Type parent,
        Type[] genericTypes,
        ref DefaultInterpolatedStringHandler nameBuilder)
    {
        int offset = 0;

        WriteNestedTypeNameTo(type, parent, genericTypes, ref nameBuilder, ref offset);
    }

    private static void WriteNestedTypeNameTo(Type type, Type parent, Type[] genericTypes,
        ref DefaultInterpolatedStringHandler nameBuilder,
        ref int offset)
    {
        if (parent.IsGenericType)
        {
            WriteNestedTypeNameTo(parent, parent.ParentType!, genericTypes, ref nameBuilder, ref offset);
        }
        else
        {
            nameBuilder.AppendLiteral(parent.Name);
        }

        nameBuilder.AppendLiteral(".");

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
            WriteNameAndGenericTypesTo(type, slice, ref nameBuilder);
        }
        else
        {
            nameBuilder.AppendLiteral(type.Name);
        }
    }

    private static void WriteNameAndGenericTypesTo(
        Type type,
        scoped ReadOnlySpan<Type> genericTypes,
        ref DefaultInterpolatedStringHandler nameBuilder)
    {
        string name = type.Name;
        int i = name.LastIndexOf('`');
        if (i >= 0)
        {
            nameBuilder.AppendFormatted(name.AsSpan(0, i));
        }
        else
        {
            nameBuilder.AppendLiteral(name);
        }

        string sep = type.IsGenericTypeDefinition ? "," : ", ";

        if (genericTypes.Length > 0)
        {
            nameBuilder.AppendLiteral("<");
            WriteTypeNameTo(genericTypes[0], ref nameBuilder);
            for (i = 1; i < genericTypes.Length; i++)
            {
                nameBuilder.AppendLiteral(sep);
                WriteTypeNameTo(genericTypes[i], ref nameBuilder);
            }
            nameBuilder.AppendLiteral(">");
        }
    }

    private static void WriteTupleNameTo(
        Type type,
        Type[]? genericTypes,
        ref DefaultInterpolatedStringHandler nameBuilder,
        bool appendParens = true)
    {
        if (appendParens)
        {
            nameBuilder.AppendLiteral("(");
        }

        genericTypes ??= type.GetGenericArguments();

        if (genericTypes.Length > 0)
        {
            writeTupleItemTypeNameTo(genericTypes[0], ref nameBuilder);
            for (int i = 1; i < genericTypes.Length; i++)
            {
                nameBuilder.AppendLiteral(", ");
                writeTupleItemTypeNameTo(genericTypes[i], ref nameBuilder);
            }
        }

        if (appendParens)
        {
            nameBuilder.AppendLiteral(")");
        }

        return;

        static void writeTupleItemTypeNameTo(Type itemType, ref DefaultInterpolatedStringHandler nb)
        {
            if (!itemType.IsTuple)
            {
                WriteTypeNameTo(itemType, ref nb);
            }
            else
            {
                WriteTupleNameTo(itemType, null, ref nb, false);
            }
        }
    }

    private static void WriteTypeNameTo(Type? type, ref DefaultInterpolatedStringHandler nameBuilder)
    {
        if (type is null)
        {
            nameBuilder.AppendLiteral("typeof(null)");
            return;
        }

        if (_cSharpSystemTypeAliases.TryGetValue(type, out var alias))
        {
            nameBuilder.AppendLiteral(alias);
            return;
        }

        if (type.IsPointer)
        {
            Debug.Assert(type.GetElementType() is not null);
            WriteTypeNameTo(type.GetElementType(), ref nameBuilder);
            nameBuilder.AppendLiteral("*");
            return;
        }

        if (type.IsByRef)
        {
            Debug.Assert(type.GetElementType() is not null);
            WriteTypeNameTo(type.GetElementType(), ref nameBuilder);
            nameBuilder.AppendLiteral("&");
            return;
        }

        if (type.IsArray)
        {
            WriteArrayTypeNameTo(type, ref nameBuilder);
            return;
        }

        Type[] genericTypes = type.GetGenericArguments();

        if (type is { IsNested: true, IsGenericParameter: false })
        {
            Type parent = type.ParentType!;
            if (parent.IsGenericType)
            {
                int offset = 0;
                WriteNestedTypeNameTo(type, parent, genericTypes, ref nameBuilder, ref offset);
                return;
            }

            WriteTypeNameTo(parent, ref nameBuilder);
            nameBuilder.AppendLiteral(".");
        }

        if (type.IsGenericType)
        {
            if (type.IsTuple)
            {
                WriteTupleNameTo(type, genericTypes, ref nameBuilder);
                return;
            }

            Type? underType = Nullable.GetUnderlyingType(type);
            if (underType is not null)
            {
                Debug.Assert(genericTypes.Length == 1);
                WriteTypeNameTo(genericTypes[0], ref nameBuilder);
                nameBuilder.AppendLiteral("?");
                return;
            }

            WriteNameAndGenericTypesTo(type, genericTypes, ref nameBuilder);
            return;
        }

        //        if (type.IsGenericParameter)
        //        {
        //            // these are part of definition, not declaration, so we don't show them
        //            // otherwise it would be T, T1, etc
        //            return;
        //        }

        nameBuilder.AppendLiteral(type.Name);
    }

    public static string For(Type? type)
    {
        DefaultInterpolatedStringHandler nameBuilder = new(64, 0);
        WriteTypeNameTo(type, ref nameBuilder);
        return nameBuilder.ToStringAndClear();
    }

    public static string For<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => For(typeof(T));

    public static string For<I>(in I? instance)
    {
        Type instanceType = instance is null ? typeof(I) : instance.GetType();
        return For(instanceType);
    }

#if NET9_0_OR_GREATER
    [EditorBrowsable(EditorBrowsableState.Never)]
    [StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
    public readonly struct Constraint;

    public static string For<I>(in I? instance, Constraint _ = default)
        where I : allows ref struct
        => For(typeof(I));
#endif
}