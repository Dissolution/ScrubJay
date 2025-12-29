
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Interpolated;

partial class TypeName
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
}