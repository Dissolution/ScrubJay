#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Universal;

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
    // We have a few local dictionaries we're going to create and reference, but never expand

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

    private static void AppendArrayType(ref InterpolatedText text, Type arrayType)
    {
        Debug.Assert(arrayType.IsArray);

        Type? elementType = arrayType.GetElementType();
        Debug.Assert(elementType is not null);

        // if we aren't a nested array, we can just print our ranks and return
        if (!elementType!.IsArray)
        {
            text.RenderType(elementType);
            text.Append('[');
            text.Append(new string(',', arrayType.GetArrayRank() - 1));
            text.Append(']');
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

        text.RenderType(elementType);
        foreach (int rank in ranks)
        {
            text.Append('[');
            text.Append(new string(',', rank - 1));
            text.Append(']');
        }
    }

    private static void AppendNameAndGenericTypes(ref InterpolatedText text,
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
            text.Append('<');
            text.RenderType(genericTypes[0]);
            for (i = 1; i < genericTypes.Length; i++)
            {
                text.Append(sep);
                text.RenderType(genericTypes[i]);
            }

            text.Append('>');
        }
    }

    private static void RenderNesting(ref InterpolatedText text, ref int offset, Type type, Type parent, Type[] genericTypes)
    {
        if (parent.IsGenericType)
        {
            RenderNesting(ref text, ref offset, parent, parent.ParentType!, genericTypes);
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
            AppendNameAndGenericTypes(ref text, type, slice);
        }
        else
        {
            text.Append(type.Name);
        }
    }

    private static void AppendComplexNestedName(
        ref InterpolatedText text,
        Type type,
        Type parent,
        Type[] genericTypes)
    {
        int offset = 0;

        RenderNesting(ref text, ref offset, type, parent, genericTypes);
    }

    private static void AppendTuple(
        ref InterpolatedText text,
        Type type,
        Type[]? genericTypes = null,
        bool appendParens = true)
    {
        if (appendParens)
        {
            text.Append('(');
        }

        genericTypes ??= type.GetGenericArguments();

        if (genericTypes.Length > 0)
        {
            Type gt = genericTypes[0];
            checkedAppend(ref text, gt);
            for (int i = 1; i < genericTypes.Length; i++)
            {
                text.Append(", ");
                checkedAppend(ref text, genericTypes[i]);
            }
        }

        if (appendParens)
        {
            text.Append(')');
        }

        return;

        static void checkedAppend(ref InterpolatedText it, Type t)
        {
            if (!t.IsTuple)
            {
                it.RenderType(t);
            }
            else
            {
                AppendTuple(ref it, t, null, false);
            }
        }
    }



    public static void RenderType(this ref InterpolatedText text, Type? type)
    {
        if (type is null)
        {
            text.Append("typeof(null)");
            return;
        }

        if (_typeAliases.TryGetValue(type, out var alias))
        {
            text.Append(alias);
            return;
        }

        if (type.IsPointer)
        {
            text.RenderType(type.GetElementType());
            text.Append('*');
            return;
        }

        if (type.IsByRef)
        {
            text.RenderType(type.GetElementType());
            text.Append('&');
            return;
        }

        if (type.IsArray)
        {
            AppendArrayType(ref text, type);
            return;
        }

        Type[] genericTypes = type.GetGenericArguments();

        if (type is { IsNested: true, IsGenericParameter: false })
        {
            var parent = type.ParentType;
            if (parent!.IsGenericType)
            {
                AppendComplexNestedName(ref text, type, parent, genericTypes);
                return;
            }

            text.RenderType(parent);
            text.Append('.');
        }

        if (type.IsGenericType)
        {
            Type genericTypeDefinition = type.GetGenericTypeDefinition();

            if (string.Equals(genericTypeDefinition.Namespace, "System", StringComparison.Ordinal) &&
                (genericTypeDefinition.Name.StartsWith("Tuple`", StringComparison.Ordinal) ||
                    genericTypeDefinition.Name.StartsWith("ValueTuple", StringComparison.Ordinal)))
            {
                AppendTuple(ref text, type, genericTypes);
                return;
            }

            if (genericTypeDefinition == typeof(Nullable<>))
            {
                Debug.Assert(genericTypes.Length == 1);
                text.RenderType(genericTypes[0]);
                text.Append('?');
                return;
            }

            AppendNameAndGenericTypes(ref text, type, genericTypes);
            return;
        }

        if (type.IsGenericParameter)
        {
            // these are part of definition, not declaration, so we don't show them
            // otherwise it would be T, T1, etc
            return;
        }

        text.Append(type.Name);
    }

    extension(Type? type)
    {
        public Type? ParentType
        {
            [return: NotNullIfNotNull(nameof(type))]
            get
            {
                if (type is not null)
                {
                    if (type.DeclaringType is not null)
                        return type.DeclaringType;
                    if (type.ReflectedType is not null)
                        return type.ReflectedType;
                    return type.Module.GetType();
                }
                return null;
            }
        }

        public bool IsTuple
        {
            get
            {
                if (type is not null)
                {
                    if (type.Namespace == "System" && (type.Name.StartsWith("Tuple") || type.Name.StartsWith("ValueTuple")))
                        return true;
                }
                return false;
            }
        }
    }

    extension(Type)
    {
        /// <summary>
        /// Gets a rendering for a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The <see cref="Type"/> to get a <see cref="TypeRenderer">rendering</see> of.
        /// </param>
        /// <returns>
        /// A <see cref="string"/> render of the <see cref="Type"/>.
        /// </returns>
        /// <seealso cref="TypeRenderer"/>
        public static string Render(Type? type)
        {
            var text = new InterpolatedText(stackalloc char[64]);
            text.RenderType(type);
            return text.ToStringAndDispose();
        }

        /// <summary>
        /// Gets a rendering for the generic type <typeparamref name="T"/>. 
        /// </summary>
        /// <typeparam name="T">
        /// The <see cref="Type"/> to get a <see cref="TypeRenderer">rendering</see> of.
        /// </typeparam>
        /// <returns>
        /// A <see cref="string"/> render of the <see cref="Type"/>.
        /// </returns>
        /// <seealso cref="TypeRenderer"/>
        public static string Render<T>()
#if NET9_0_OR_GREATER
            where T : allows ref struct
#endif
        {
            var text = new InterpolatedText(stackalloc char[64]);
            text.RenderType(typeof(T));
            return text.ToStringAndDispose();
        }

        public static string Render<I>(in I? instance)
        {
            var text = new InterpolatedText(stackalloc char[64]);
            text.RenderType(Any.GetType<I>(in instance));
            return text.ToStringAndDispose();
        }

#if NET9_0_OR_GREATER
        public static string Render<I>(in I? instance, TypeConstraints.AllowsRefStruct<I> _ = default)
            where I : allows ref struct
        {
            var text = new InterpolatedText(stackalloc char[64]);
            text.RenderType(Any.GetType<I>(in instance, _));
            return text.ToStringAndDispose();
        }
#endif
    }
}