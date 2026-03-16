using ScrubJay.Universal;

namespace ScrubJay.Enums;

[PublicAPI]
public abstract class EnumTypeInfo : IEqualityComparer<Enum>, IComparer<Enum>
{
    public Type EnumType { get; }
    public Type UnderlyingType { get; }
    public bool IsSigned { get; }
    public Attribute[] Attributes { get; }
    public bool IsFlags { get; }

    protected EnumTypeInfo(Type enumType)
    {
        Debug.Assert(enumType.IsEnum);
        this.EnumType = enumType;
        this.UnderlyingType = enumType.GetEnumUnderlyingType()!;
        this.IsSigned = UnderlyingType.GetTypeCode() is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
        this.Attributes = Attribute.GetCustomAttributes(EnumType, true);
        this.IsFlags = Attributes.OfType<FlagsAttribute>().Any();
    }

    public Result<Enum> TryParse(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
    {
        throw new NotImplementedException();
    }

    public Result<Enum> TryParse(string? str, bool ignoreCase = true, bool includeAttributes = true)
    {
        throw new NotImplementedException();
    }

    public Result<Enum> TryParse(long i64)
    {
        throw new NotImplementedException();
    }

    public Result<Enum> TryParse(ulong u64)
    {
        throw new NotImplementedException();
    }

    public Result<Enum> TryParse(object? obj,
        bool ignoreCase = true,
        bool useAttributes = true)
    {
        throw new NotImplementedException();
    }
    
    



    public abstract int Compare(Enum x, Enum y);

    public abstract bool Equals(Enum x, Enum y);

    public abstract int GetHashCode(Enum obj);

    public sealed override string ToString() => EnumType.Render();
}

[PublicAPI]
public sealed class EnumTypeInfo<TEnum> : EnumTypeInfo,
    IEqualityComparer<TEnum>, IComparer<TEnum>
    where TEnum : struct, Enum
{
    internal EnumTypeInfo() : base(typeof(TEnum))
    {
    }

    public Result<TEnum> TryParse(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
    {
        throw new NotImplementedException();
    }

    public Result<TEnum> TryParse(string? str, bool ignoreCase = true, bool includeAttributes = true)
    {
        throw new NotImplementedException();
    }

    public Result<TEnum> TryParse(long i64)
    {
        throw new NotImplementedException();
    }

    public Result<TEnum> TryParse(ulong u64)
    {
        throw new NotImplementedException();
    }

    public Result<TEnum> TryParse(object? obj,
        bool ignoreCase = true,
        bool useAttributes = true)
    {
        throw new NotImplementedException();
    }
    
    
    
    
    
    
    public override int Compare(Enum x, Enum y) => throw new NotImplementedException();

    public override bool Equals(Enum x, Enum y) => throw new NotImplementedException();

    public override int GetHashCode(Enum obj) => throw new NotImplementedException();

    public bool Equals(TEnum x, TEnum y) => throw new NotImplementedException();

    public int GetHashCode(TEnum obj) => throw new NotImplementedException();

    public int Compare(TEnum x, TEnum y) => throw new NotImplementedException();
}