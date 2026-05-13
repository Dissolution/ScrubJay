namespace ScrubJay.Enums;

public abstract partial class EnumTypeInfo : IEqualityComparer<Enum>, IComparer<Enum>
{
    public Type EnumType { get; }
    public Type EnumUnderlyingType { get; }
    public bool IsSigned { get; }
    public abstract int Size { get; }
    public Attribute[] Attributes { get; }

    public string[] MemberNames { get; }

    protected EnumTypeInfo(Type enumType)
    {
        Debug.Assert(enumType is not null);
        Debug.Assert(enumType!.IsEnum);
        this.EnumType = enumType;
        this.EnumUnderlyingType = enumType.GetEnumUnderlyingType();
        this.IsSigned = Type.GetTypeCode(EnumUnderlyingType) is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
        this.Attributes = Attribute.GetCustomAttributes(EnumType, true);
        this.MemberNames = Enum.GetNames(EnumType);
    }

    public abstract Result<Enum> TryParseEnum(scoped text text, bool ignoreCase = true, bool includeAttributes = true);

    public abstract Result<Enum> TryParseEnum(string? str, bool ignoreCase = true, bool includeAttributes = true);

    public abstract Result<Enum> TryParseEnum(long i64);

    public abstract Result<Enum> TryParseEnum(ulong u64);

    public abstract Result<Enum> TryParseEnum(object? obj,
        bool ignoreCase = true,
        bool useAttributes = true);

    public abstract string? GetName(Enum? @enum);

    public abstract string? Format(Enum? @enum, string? format = default);

    public abstract Option<int> TryFormatTo(Enum? @enum, Span<char> destination, string? format = default);

    public abstract ulong? ToU64(Enum? @enum);

    public abstract long? ToI64(Enum? @enum);

    public bool IsDefined(Enum? @enum)
    {
        return @enum is not null && Enum.IsDefined(EnumType, @enum);
    }

    public abstract EnumMemberInfo? GetMemberInfo(Enum? @enum);

    public abstract Option<EnumMemberInfo> TryGetMemberInfo(Enum? @enum);

    public abstract int Compare(Enum? left, Enum? right);

    public abstract bool Equals(Enum? left, Enum? right);

    public abstract int GetHashCode(Enum? @enum);

    public sealed override string ToString()
    {
        return EnumType.Name;
    }
}