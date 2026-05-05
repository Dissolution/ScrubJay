// Runtime dispatch entry point — use when you only have an Enum instance

using System.Collections.Concurrent;
using System.Reflection;


internal abstract partial class EnumTypeInfo
{
    private static readonly ConcurrentDictionary<Type, EnumTypeInfo> _cache = new();

    public static EnumTypeInfo For(Enum value) => For(value.GetType());
    
    public static EnumTypeInfo For(Type type) =>
        _cache.GetOrAdd(type, static t =>
            (EnumTypeInfo)typeof(EnumTypeInfo<>)
                .MakeGenericType(t)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)!
                .GetValue(null)!);
}

internal abstract partial class EnumTypeInfo : IEqualityComparer<Enum>, IComparer<Enum>
{
    public abstract Type EnumType { get; }
    
    public abstract Result<Enum> TryParseEnum(scoped text text, bool ignoreCase = true, bool includeAttributes = true);

    public abstract Result<Enum> TryParseEnum(string? str, bool ignoreCase = true, bool includeAttributes = true);

    public abstract Result<Enum> TryParseEnum(long i64);

    public abstract Result<Enum> TryParseEnum(ulong u64);

    public abstract Result<Enum> TryParseEnum(object? obj,
        bool ignoreCase = true,
        bool useAttributes = true);

    public abstract string? GetName(Enum? @enum);

    public abstract string? Format(Enum? @enum, string? format = default);

    public abstract Result<int> TryFormat(Enum? @enum, Span<char> destination, string? format = default);

    public bool IsDefined(Enum? @enum)
    {
        return @enum is not null && Enum.IsDefined(EnumType, @enum);
    }

    public abstract EnumMemberInfo? GetMemberInfo(Enum? @enum);

    public abstract Result<EnumMemberInfo> TryGetMemberInfo(Enum? @enum);

    public abstract int Compare(Enum? x, Enum? y);

    public abstract bool Equals(Enum? x, Enum? y);

    public abstract int GetHashCode(Enum? obj);

    public sealed override string ToString()
    {
        return EnumType.Name;
    }
}

internal sealed partial class EnumTypeInfo<TEnum>
{
    public static readonly EnumTypeInfo<TEnum> Instance = new();
}

internal sealed partial class EnumTypeInfo<TEnum> : EnumTypeInfo 
    where TEnum : struct, Enum
{
    public readonly Type EnumType = typeof(TEnum);
    public readonly Type UnderlyingType = typeof(TEnum).GetEnumUnderlyingType();
    public readonly bool IsSigned;
    
    public readonly string[] MemberNames = Enum.GetNames<TEnum>();
    public readonly TEnum[] MemberValues = Enum.GetValues<TEnum>();
    public readonly IReadOnlyDictionary<TEnum, EnumMemberInfo<TEnum>> Members = GetMembers();
    public readonly Attribute[] Attributes = Attribute.GetCustomAttributes(typeof(TEnum));
   
    private EnumTypeInfo()
    {
        IsSigned = Type.GetTypeCode(UnderlyingType) is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
    }
}

internal abstract partial class EnumMemberInfo
{
    public abstract Enum AsEnum();
}

internal sealed partial class EnumMemberInfo<TEnum> : EnumMemberInfo
    where TEnum : struct, Enum
{
    public readonly string MemberName;
    public readonly TEnum Member;
    public readonly Attribute[] Attributes;
}