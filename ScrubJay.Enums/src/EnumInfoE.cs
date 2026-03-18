using System.Globalization;
using System.Reflection;
using ScrubJay.Universal;

namespace ScrubJay.Enums;

internal abstract class EnumTypeInfo : IEqualityComparer<Enum>, IComparer<Enum>
{
    protected readonly EnumMemberInfo[] _enumMemberInfos;

    public Type EnumType { get; }
    public Type UnderlyingType { get; }
    public bool IsSigned { get; }
    public Attribute[] Attributes { get; }
    public bool IsFlags { get; }

    public string[] MemberNames
    {
        get
        {
            return EnumType.GetEnumNames();
        }
    }

    public Enum[] Enums
    {
        get
        {
            return (Enum[])EnumType.GetEnumValues();
        }
    }

    public EnumMemberInfo[] MemberInfos
    {
        get
        {
            return _enumMemberInfos;
        }
    }

    protected EnumTypeInfo(Type enumType, EnumMemberInfo[] enumMemberInfos)
    {
        Debug.Assert(enumType.IsEnum);
        _enumMemberInfos = enumMemberInfos;
        this.EnumType = enumType;
        this.UnderlyingType = enumType.GetEnumUnderlyingType()!;
        this.IsSigned = UnderlyingType.GetTypeCode() is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
        this.Attributes = Attribute.GetCustomAttributes(EnumType, true);
        this.IsFlags = Attributes.OfType<FlagsAttribute>().Any();
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
        return EnumType.Render();
    }
}

internal sealed class EnumTypeInfo<TEnum> : EnumTypeInfo,
    IEqualityComparer<TEnum>, IComparer<TEnum>
    where TEnum : struct, Enum
{
    private static EnumMemberInfo<TEnum>[] CreateEnumMemberInfos()
    {
        var enumType = typeof(TEnum);
        var memberFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
        int count = memberFields.Length;
        var memberInfos = new EnumMemberInfo<TEnum>[count];
        for (var i = 0; i < count; i++)
        {
            memberInfos[i] = new EnumMemberInfo<TEnum>(memberFields[i]);
        }
        return memberInfos;
    }


    public TEnum[] Members
    {
        get
        {
#if NET6_0_OR_GREATER
            return Enum.GetValues<TEnum>();
#else
            Array valuesArray = Enum.GetValues(EnumType);
            Enum[] enumArray = (Enum[])valuesArray;
            TEnum[] tenumArray = (TEnum[])valuesArray;
            Debugger.Break();
            return tenumArray;
#endif
        }
    }

    // ReSharper disable once CoVariantArrayConversion
    public new EnumMemberInfo<TEnum>[] MemberInfos
    {
        get
        {
            return (EnumMemberInfo<TEnum>[])_enumMemberInfos;
        }
    }

    // ReSharper disable once CoVariantArrayConversion
    internal EnumTypeInfo() : base(typeof(TEnum), CreateEnumMemberInfos())
    {
    }

    // text

    public Result<TEnum> TryParse(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
    {
        throw new NotImplementedException();
    }

    public override Result<Enum> TryParseEnum(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
    {
        var result = TryParse(text, ignoreCase, includeAttributes);
        return result.Select(static e => (Enum)e);
    }

    // str

    public Result<TEnum> TryParse(string? str, bool ignoreCase = true, bool includeAttributes = true)
    {
        throw new NotImplementedException();
    }

    public override Result<Enum> TryParseEnum(string? str, bool ignoreCase = true, bool includeAttributes = true)
    {
        var result = TryParse(str, ignoreCase, includeAttributes);
        return result.Select(static e => (Enum)e);
    }

    // long

    public Result<TEnum> TryParse(long i64)
    {
        throw new NotImplementedException();
    }

    public override Result<Enum> TryParseEnum(long i64)
    {
        var result = TryParse(i64);
        return result.Select(static e => (Enum)e);
    }

    // ulong

    public Result<TEnum> TryParse(ulong u64)
    {
        throw new NotImplementedException();
    }

    public override Result<Enum> TryParseEnum(ulong u64)
    {
        var result = TryParse(u64);
        return result.Select(static e => (Enum)e);
    }

    // object

    public Result<TEnum> TryParse(object? obj,
        bool ignoreCase = true,
        bool useAttributes = true)
    {
        throw new NotImplementedException();
    }

    public override Result<Enum> TryParseEnum(object? obj, bool ignoreCase = true, bool includeAttributes = true)
    {
        var result = TryParse(obj, ignoreCase, includeAttributes);
        return result.Select(static e => (Enum)e);
    }

    // getname

    public string GetName(TEnum @enum)
    {
        throw new NotImplementedException();
    }

    public override string? GetName(Enum? @enum)
    {
        if (@enum is TEnum tenum)
        {
            return GetName(tenum);
        }
        return null;
    }

    // format
    
    public string Format(TEnum @enum, string? format = default)
    {
        throw new NotImplementedException();
    }

    public override string? Format(Enum? @enum, string? format = default)
    {
        if (@enum is TEnum tenum)
        {
            return Format(tenum, format);
        }
        return null;
    }

    // tryformat
    
    public Result<int> TryFormat(TEnum @enum, Span<char> destination, string? format = default)
    {
        throw new NotImplementedException();
    }
    
    public override Result<int> TryFormat(Enum? @enum, Span<char> destination, string? format = default)
    {
        if (@enum is TEnum tenum)
        {
            return TryFormat(tenum, destination, format);
        }
        if (@enum is null)
            return new ArgumentNullException(nameof(@enum));
        var ex = new ArgumentException($"Enum", nameof(@enum));
        return ex;
    }

    public override EnumMemberInfo? GetMemberInfo(Enum? @enum)
    {
        throw new NotImplementedException();
    }

    public override Result<EnumMemberInfo> TryGetMemberInfo(Enum? @enum)
    {
        throw new NotImplementedException();
    }





    public bool IsDefined(TEnum @enum)
    {
#if NET6_0_OR_GREATER
        return Enum.IsDefined<TEnum>(@enum);
#else
        return Enum.IsDefined(EnumType, (object)@enum);
#endif
    }




    public override int Compare(Enum? x, Enum? y)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(Enum? x, Enum? y)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode(Enum? obj)
    {
        throw new NotImplementedException();
    }

    public bool Equals(TEnum x, TEnum y)
    {
        throw new NotImplementedException();
    }

    public int GetHashCode(TEnum obj)
    {
        throw new NotImplementedException();
    }

    public int Compare(TEnum x, TEnum y)
    {
        throw new NotImplementedException();
    }
}