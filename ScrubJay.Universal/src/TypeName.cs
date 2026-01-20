#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Universal;

[PublicAPI]
public static partial class TypeName
{
    // We have a few local dictionaries we're going to create and reference, but never expand

#if NET8_0_OR_GREATER
    private static readonly FrozenDictionary<Type, string> _typeAliases;
#else
    private static readonly Dictionary<Type, string> _typeAliases;
#endif

    static TypeName()
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
                    [typeof(Tuple)] = "()",
                    [typeof(ValueTuple)] = "()",
                }
#if NET8_0_OR_GREATER
                .ToFrozenDictionary()
#endif
            ;
    }


    internal static StringBuilder AppendTypeName(this StringBuilder builder, Type? type)
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
            var parent = type.ParentType;
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
}