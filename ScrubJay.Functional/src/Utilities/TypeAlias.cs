using System.Text;
using ScrubJay.Functional.Extensions;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Functional.Utilities;

[PublicAPI]
public static class TypeAlias
{
    // We have a few local dictionaries we're going to create and reference, but never expand
#if NET8_0_OR_GREATER
    private static readonly FrozenDictionary<Type, string> _typeAliases;
#else
    private static readonly Dictionary<Type, string> _typeAliases;
#endif

    static TypeAlias()
    {
        _typeAliases = new Dictionary<Type, string>
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
                [typeof(ValueTuple)] = "()",
            }
#if NET8_0_OR_GREATER
                .ToFrozenDictionary()
#endif
            ;
    }

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

    private static StringBuilder AppendNameAndGenericTypes(
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

    private static StringBuilder AppendArrayType(
        this StringBuilder builder,
        Type arrayType)
    {
        Debug.Assert(arrayType.IsArray);

        Type? elementType = arrayType.GetElementType();
        Debug.Assert(elementType is not null);

        if (!elementType!.IsArray)
        {
            return builder
                .AppendTypeName(elementType)
                .Append('[')
                .Append(',', arrayType.GetArrayRank() - 1)
                .Append(']');
        }

        // we need to print the root element type, then the array ranks in order from outmost to inmost
        Queue<int> ranks = new();
        ranks.Enqueue(arrayType.GetArrayRank());

        while (elementType is not null && elementType.IsArray)
        {
            ranks.Enqueue(elementType.GetArrayRank());
            elementType = elementType.GetElementType();
        }

        builder.AppendTypeName(elementType);
        foreach (int rank in ranks)
        {
            builder.Append('[')
                .Append(',', rank - 1)
                .Append(']');
        }

        return builder;
    }

    private static bool IsGenericTuple(Type type, Type? genericTypeDefinition = null)
    {
        if (genericTypeDefinition is null)
        {
            if (!type.IsGenericType)
                return false;
            genericTypeDefinition = type.GetGenericTypeDefinition();
        }

        return genericTypeDefinition.Namespace == "System" &&
            (genericTypeDefinition.Name.StartsWith("ValueTuple") ||
                genericTypeDefinition.Name.StartsWith("Tuple"));
    }

    private static void WriteTuple(
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

    private static StringBuilder AppendTypeName(this StringBuilder builder, Type? type)
    {
        if (type is null)
            return builder.Append("null");

        if (_typeAliases.TryGetValue(type, out var alias))
            return builder.Append(alias);

        if (type.IsPointer)
        {
            return builder.AppendTypeName(type.GetElementType()).Append('*');
        }

        if (type.IsByRef)
        {
            return builder.AppendTypeName(type.GetElementType()).Append('&');
        }

        if (type.IsArray)
        {
            return builder.AppendArrayType(type);
        }

        Type[] genericTypes = type.GetGenericArguments();

        if (type is { IsNested: true, IsGenericParameter: false })
        {
            var parent = type!.ParentType!;
            if (parent.IsGenericType)
            {
                return AppendComplexNestedName(builder, type, parent, genericTypes);
            }

            builder.AppendTypeName(parent).Append('.');
        }

        if (type.IsGenericType)
        {
            Type genericTypeDefinition = type.GetGenericTypeDefinition();

            if (IsGenericTuple(type, genericTypeDefinition))
            {
                WriteTuple(builder, type, genericTypes);
                return builder;
            }

            if (genericTypeDefinition == typeof(Nullable<>))
            {
                Debug.Assert(genericTypes.Length == 1);
                return builder.AppendTypeName(genericTypes[0]).Append('?');
            }

            return AppendNameAndGenericTypes(builder, type, genericTypes);
        }

        if (type.IsGenericParameter)
        {
            // these are part of definition, not declaration, so we don't show them
            // otherwise it would be T, T1, etc
            return builder;
        }

        return builder.Append(type.Name);
    }

    public static string For(Type? type)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendTypeName(type);
        return builder.ToString();
    }

    public static string For<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => For(typeof(T));

    public static string For<I>(in I? instance)
    {
        if (instance is null)
            return For(typeof(I));
        return For(instance.GetType());
    }
}