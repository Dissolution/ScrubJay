#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Text.Rendering;

/// <summary>
/// A utility for rendering <see cref="Type"/>s.
/// </summary>
/// <remarks>
/// Rendering a <see cref="Type"/> is getting a <see cref="string"/> representation that is more<br/>
/// readable to a C# developer.<br/>
///<br/>
/// Examples:
/// <code>
/// </code>
/// </remarks>
[PublicAPI]
public static class TypeRenderer
{
    /// <summary>
    /// C# type aliases to quickly resolve
    /// </summary>
#if NET8_0_OR_GREATER
    private static readonly FrozenDictionary<Type, string> _typeAliases;
#else
    private static readonly Dictionary<Type, string> _typeAliases;
#endif

    static TypeRenderer()
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

    private static void WriteArrayTypeTo(Type arrayType, TextBuilder text)
    {
        Debug.Assert(arrayType.IsArray);

        Type? elementType = arrayType.GetElementType();
        Debug.Assert(elementType is not null);

        // if we aren't a nested array, we can just print our ranks and return
        if (!elementType!.IsArray)
        {
            text.Render(elementType)
                .Append('[')
                .Repeat(arrayType.GetArrayRank() - 1, ',')
                .Append(']');
            return;
        }

        // we need to print the root element type, then the array ranks in order from outmost to inmost
        Queue<int> ranks = new();
        ranks.Enqueue(arrayType.GetArrayRank());

        while (elementType is not null && elementType.IsArray)
        {
            ranks.Enqueue(elementType.GetArrayRank());
            elementType = elementType.GetElementType();
        }

        text.Render(elementType);
        foreach (int rank in ranks)
        {
            text.Append('[')
                .Repeat(rank - 1, ',')
                .Append(']');
        }
    }

    private static void WriteNameAndGenericTypes(
        TextBuilder text,
        Type type,
        params ReadOnlySpan<Type> genericTypes)
    {
        int i = type.Name.LastIndexOf('`');
        if (i >= 0)
        {
            text.Append(type.Name.AsSpan(0, i));
        }
        else
        {
            text.Append(type.Name);
        }

        string sep = type.IsGenericTypeDefinition ? "," : ", ";

        if (genericTypes.Length > 0)
        {
            text.Append('<')
                .Delimit(sep, genericTypes, TB.Render)
                .Append('>');
        }
    }

    private static void RenderTypeNesting(TextBuilder text, ref int offset, Type type, Type parent, Type[] genericTypes)
    {
        if (parent.IsGenericType)
        {
            RenderTypeNesting(text, ref offset, parent, parent.ParentType!, genericTypes);
        }
        else
        {
            text.Append(parent.Name);
        }

        text.Append('.');

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
            WriteNameAndGenericTypes(text, type, slice);
        }
        else
        {
            text.Append(type.Name);
        }
    }

    private static void RenderComplexNestedName(
        TextBuilder text,
        Type type,
        Type parent,
        Type[] genericTypes)
    {
        int offset = 0;

        RenderTypeNesting(text, ref offset, type, parent, genericTypes);
    }

    private static void WriteTuple(
        TextBuilder text,
        Type type,
        Type[]? genericTypes = null,
        bool appendParens = true)
    {
        Debug.Assert(type.IsTuple);

        text
            .If(appendParens, TB.Append('('))
            .Delimit(", ", genericTypes ?? type.GetGenericArguments(), checkedAppend)
            .If(appendParens, TB.Append(')'));
        return;

        static void checkedAppend(TextBuilder it, Type t)
        {
            if (!t.IsTuple)
            {
                it.Render(t);
            }
            else
            {
                WriteTuple(it, t, null, false);
            }
        }
    }

    [RenderToMethod]
    public static void RenderTypeTo(Type? type, TextBuilder builder)
    {
        if (type is null)
        {
            builder.Append("typeof(null)");
            return;
        }

        if (_typeAliases.TryGetValue(type, out var alias))
        {
            builder.Append(alias);
            return;
        }

        if (type.IsPointer)
        {
            builder.Render(type.GetElementType());
            builder.Append('*');
            return;
        }

        if (type.IsByRef)
        {
            builder.Render(type.GetElementType());
            builder.Append('&');
            return;
        }

        if (type.IsArray)
        {
            WriteArrayTypeTo(type, builder);
            return;
        }

        Type[] genericTypes = type.GetGenericArguments();

        if (type is { IsNested: true, IsGenericParameter: false })
        {
            var parent = type.ParentType;
            if (parent!.IsGenericType)
            {
                RenderComplexNestedName(builder, type, parent, genericTypes);
                return;
            }

            builder.Render(parent);
            builder.Append('.');
        }

        if (type.IsGenericType)
        {
            Type genericTypeDefinition = type.GetGenericTypeDefinition();

            if (string.Equals(genericTypeDefinition.Namespace, "System", StringComparison.Ordinal) &&
                (genericTypeDefinition.Name.StartsWith("Tuple`", StringComparison.Ordinal) ||
                    genericTypeDefinition.Name.StartsWith("ValueTuple", StringComparison.Ordinal)))
            {
                WriteTuple(builder, type, genericTypes);
                return;
            }

            if (genericTypeDefinition == typeof(Nullable<>))
            {
                Debug.Assert(genericTypes.Length == 1);
                builder.Render(genericTypes[0]);
                builder.Append('?');
                return;
            }

            WriteNameAndGenericTypes(builder, type, genericTypes);
            return;
        }

//        if (type.IsGenericParameter)
//        {
//            // these are part of definition, not declaration, so we don't show them
//            // otherwise it would be T, T1, etc
//            return;
//        }

        builder.Append(type.Name);
    }
}