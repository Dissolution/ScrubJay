using System.Text;
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Interpolated;

[PublicAPI]
public static class TypeName
{
    // We have a few local dictionaries we're going to create and reference, but never expand

#if NET8_0_OR_GREATER
    private static readonly FrozenSet<Type> _tupleTypes;
    private static readonly FrozenDictionary<Type, string> _typeAliases;
#else
    private static readonly HashSet<Type> _tupleTypes;
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

        _tupleTypes = new HashSet<Type>
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
    }

    public static StringBuilder AppendType(this StringBuilder builder, Type? type)
    {
        if (type is null)
            return builder.Append("null");
        
        // fast check for c# type aliases
        if (_typeAliases.TryGetValue(type, out var name))
        {
            return builder.Append(name);
        }

        Type? underType;

        // ref T
        if (type.IsByRef)
        {
            // display as `type&`
            Debug.Assert(type.GetGenericArguments().Length == 0);
            underType = type.GetElementType()!;
            Debug.Assert(underType is not null);
            return builder.AppendType(underType).Append('&');
        }

        // pointers
        if (type.IsPointer)
        {
            // display as `type*`

            Debug.Assert(type.GetGenericArguments().Length == 0);
            underType = type.GetElementType()!;
            Debug.Assert(underType is not null);
            return builder.AppendType(underType).Append('*');
        }

        // arrays (N-Dimensional)
        if (type.IsArray)
        {
            int rank = type.GetArrayRank();
            Debug.Assert(type.GetGenericArguments().Length == 0);
            underType = type.GetElementType()!;
            Debug.Assert(underType is not null);

            return builder
                .AppendType(underType)
                .Append('[')
                .Append(',', repeatCount: rank - 1)
                .Append(']');
        }

        // Gather any generic types
        Span<Type> genericTypes = type.GetGenericArguments();

        // generic types we need to display that information specially
        if (type.IsGenericType)
        {
            Debug.Assert(genericTypes.Length > 0);
            var genericTypeDefinition = type.GetGenericTypeDefinition();

            // Nullable<struct>
            if (genericTypeDefinition == typeof(Nullable<>))
            {
                // display as `type?`
                Debug.Assert(genericTypes.Length == 1);
                underType = genericTypes[0];
                return builder.AppendType(underType).Append('?');
            }

            // Tuple/ValueTuple?
            if (_tupleTypes.Contains(genericTypeDefinition))
            {
                // display as `(type, type,..)`

                builder.Append('(')
                    .AppendType(genericTypes[0]);
                for (int i = 1; i < genericTypes.Length; i++)
                {
                    builder.Append(", ")
                        .AppendType(genericTypes[i]);
                }

                return builder.Append(')');
            }
        }

        // Nested types need special handling -- especially for generics
        if (type.IsNested && !type.IsGenericParameter)
        {
            Debug.Assert(type.DeclaringType is not null);
            var declaringType = type.DeclaringType!;
            var declaringTypeGenericTypes = declaringType.GetGenericArguments();
            if (declaringTypeGenericTypes.Length > 0)
            {
                // we have to account for these generic types in our overall list of generic types
                Debug.Assert(genericTypes.Length >= declaringTypeGenericTypes.Length);
                genericTypes = genericTypes[declaringTypeGenericTypes.Length..];
            }

            // show as `declaringType.`
            builder.AppendType(declaringType).Append('.');
        }

        if (genericTypes.Length > 0)
        {
            Debug.Assert(type.IsGenericType);

            // name might have a ` in it  (IList<int> shows as IList`1)
            int i = type.Name.IndexOf('`');
            if (i >= 0)
            {
                builder.Append(type.Name.AsSpan(0, i));
            }
            else
            {
                builder.Append(type.Name);
            }

            builder.Append('<').AppendType(genericTypes[0]);
            for (i = 1; i < genericTypes.Length; i++)
            {
                builder.Append(", ").Append(genericTypes[i]);
            }
            return builder.Append('>');
        }

        // just the name
        return builder.Append(type.Name);
    }

    public static string For(Type? type)
    {
        return new StringBuilder()
            .AppendType(type)
            .ToString();
    }
    
    public static string For<T>() => For(typeof(T));

}