using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using ScrubJay.Enums.Extensions;


#if !NETFRAMEWORK && !NETSTANDARD
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
#endif
#if NET8_0_OR_GREATER
using System.Collections.Frozen;
#endif

namespace ScrubJay.Enums;

[PublicAPI]
public static class EnumInfoExtensions
{
    private static readonly ConcurrentDictionary<Type, EnumTypeInfo> _cache = new();

    private static EnumTypeInfo CacheGet(Type enumType)
    {
        Debug.Assert(enumType is not null);
        Debug.Assert(enumType.IsEnum);
        return _cache.GetOrAdd(enumType, static t =>
            (EnumTypeInfo)typeof(EnumTypeInfo<>)
                .MakeGenericType(t)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)!
                .GetValue(null)!);
    }

    extension(Enum)
    {
        public static EnumTypeInfo? GetInfo(Enum? @enum)
        {
            if (@enum is not null)
            {
                return CacheGet(@enum.GetType());
            }
            return null;
        }

        public static EnumTypeInfo? GetInfo(Type? enumType)
        {
            if (enumType is not null && enumType.IsEnum)
            {
                return CacheGet(enumType);
            }
            return null;
        }

        public static Result<EnumTypeInfo> TryGetInfo(Enum? @enum)
        {
            if (@enum is null)
                return new ArgumentNullException(nameof(@enum));
            var enumType = @enum.GetType();
            return CacheGet(enumType);
        }

        public static Result<EnumTypeInfo> TryGetInfo(Type? enumType)
        {
            if (enumType is null)
                return new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum)
                return new ArgumentException(null, nameof(enumType));
            return CacheGet(enumType);
        }

        public static EnumTypeInfo<TEnum> GetInfo<TEnum>()
            where TEnum : struct, Enum
        {
            return EnumTypeInfo<TEnum>.Instance;
        }

        public static EnumTypeInfo<TEnum> GetInfo<TEnum>(TEnum _)
            where TEnum : struct, Enum
        {
            return EnumTypeInfo<TEnum>.Instance;
        }

    }
}

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
        Debug.Assert(enumType.IsEnum);
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

public sealed partial class EnumTypeInfo<TEnum>
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

    public static readonly EnumTypeInfo<TEnum> Instance = new();
}

public sealed partial class EnumTypeInfo<TEnum> : EnumTypeInfo, IEqualityComparer<TEnum>, IComparer<TEnum>
    where TEnum : struct, Enum
{
    public readonly TEnum[] Members =
#if NETFRAMEWORK || NETSTANDARD
            (TEnum[])Enum.GetValues(typeof(TEnum))
#else
            Enum.GetValues<TEnum>()
#endif
        ;

    public readonly EnumMemberInfo<TEnum>[] MemberInfos = CreateEnumMemberInfos();

    public override int Size { get; } = Unsafe.SizeOf<TEnum>();

    internal EnumTypeInfo() : base(typeof(TEnum))
    {

    }

#region TryParse
    public override Result<Enum> TryParseEnum(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
    {
        return TryParse(text, ignoreCase, includeAttributes).Select(e => (Enum)e);
    }

    public override Result<Enum> TryParseEnum(string? str, bool ignoreCase = true, bool includeAttributes = true)
    {
        return TryParse(str, ignoreCase, includeAttributes).Select(e => (Enum)e);
    }

    public override Result<Enum> TryParseEnum(long i64)
    {
        return TryParse(i64).Select(e => (Enum)e);
    }

    public override Result<Enum> TryParseEnum(ulong u64)
    {
        return TryParse(u64).Select(e => (Enum)e);
    }

    public override Result<Enum> TryParseEnum(object? obj, bool ignoreCase = true, bool useAttributes = true)
    {
        return TryParse(obj, ignoreCase, useAttributes).Select(e => (Enum)e);
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

    public Result<TEnum> TryParse(object? obj, bool ignoreCase = true, bool useAttributes = true)
    {
        throw new NotImplementedException();
    }
#endregion

    public override string? GetName(Enum? @enum)
    {
        if (@enum is TEnum t)
            return GetName(t);
        return @enum?.ToString();
    }

    public string? GetName(TEnum @enum)
    {
        throw new NotImplementedException();
    }

    public override ulong? ToU64(Enum? @enum)
    {
        if (@enum is TEnum t)
            return ToU64(t);
        return null;
    }

    public ulong ToU64(TEnum @enum)
    {
        ulong value = Unsafe.As<TEnum, ulong>(ref @enum);
        return value;
    }

    public override long? ToI64(Enum? @enum)
    {
        if (@enum is TEnum t)
            return ToI64(t);
        return null;
    }

    public long ToI64(TEnum @enum)
    {
        long value = Unsafe.As<TEnum, long>(ref @enum);
        return value;
    }

    public override string? Format(Enum? @enum, string? format = default) => throw new NotImplementedException();

    public string? Format(TEnum @enum, string? format = default) => throw new NotImplementedException();


    public override Option<int> TryFormatTo(Enum? @enum, Span<char> destination, string? format = default) => throw new NotImplementedException();

    public Option<int> TryFormatTo(TEnum @enum, Span<char> destination, string? format = default) => throw new NotImplementedException();


#region Equate / Compare
    public override bool Equals(Enum? left, Enum? right)
    {
        return left is TEnum leftT && right is TEnum rightT && Equals(leftT, rightT);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(TEnum left, TEnum right)
    {
        Emit.Ldarg(nameof(left));
        Emit.Ldarg(nameof(right));
        Emit.Ceq();
        return Return<bool>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool NotEquals(TEnum left, TEnum right)
    {
        Emit.Ldarg(nameof(left));
        Emit.Ldarg(nameof(right));
        Emit.Ceq();
        Emit.Ldc_I4_0();
        Emit.Ceq();
        return Return<bool>();
    }

    public override int GetHashCode(Enum? @enum)
    {
        if (@enum is TEnum t)
            return GetHashCode(t);
        if (@enum is null)
            return 0;
        return @enum.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(TEnum @enum)
    {
        ulong value = Unsafe.As<TEnum, ulong>(ref @enum);
        return (int)value ^ (int)(value >> 32);
    }

    public override int Compare(Enum? left, Enum? right)
    {
        if (left is TEnum leftT)
        {
            if (right is TEnum rightT)
            {
                return Compare(leftT, rightT);
            }
            return 1;
        }
        return right is TEnum ? -1 : 0;
    }

    public int Compare(TEnum left, TEnum right)
    {
        if (IsSigned)
        {
            long l = Unsafe.As<TEnum, long>(ref left);
            long r = Unsafe.As<TEnum, long>(ref right);
            return (l > r ? 1 : 0) - (l < r ? 1 : 0);
        }
        else
        {
            ulong l = Unsafe.As<TEnum, ulong>(ref left);
            ulong r = Unsafe.As<TEnum, ulong>(ref right);
            return (l > r ? 1 : 0) - (l < r ? 1 : 0);
        }
    }
#endregion

    public override EnumMemberInfo? GetMemberInfo(Enum? @enum)
    {
        if (@enum is TEnum t)
            return GetMemberInfo(t);
        return null;
    }

    public EnumMemberInfo<TEnum>? GetMemberInfo(TEnum @enum)
    {
        return this.MemberInfos.FirstOrDefault(info => info.Member == @enum);
    }

    public override Option<EnumMemberInfo> TryGetMemberInfo(Enum? @enum)
    {
        if (@enum is TEnum t)
            return TryGetMemberInfo(t).Select(e => (EnumMemberInfo)e);
        return None;
    }

    public Option<EnumMemberInfo<TEnum>> TryGetMemberInfo(TEnum @enum)
    {
        var info = this.MemberInfos.FirstOrDefault(info => info.Member == @enum);
        return Option.NotNull(info);
    }
}

public abstract partial class EnumMemberInfo
{
    public abstract Enum MemberEnum { get; }
}

public sealed partial class EnumMemberInfo<TEnum> : EnumMemberInfo
    where TEnum : struct, Enum
{
#if NET8_0_OR_GREATER
    protected readonly FrozenDictionary<string, string> _aliases;
#elif NET6_0_OR_GREATER
    protected readonly ImmutableDictionary<string, string> _aliases;
#else
    protected readonly ReadOnlyDictionary<string, string> _aliases;
#endif

    public readonly string MemberName;
    public readonly TEnum Member;
    public readonly Attribute[] Attributes;

    public override Enum MemberEnum => (Enum)Member;

    internal EnumMemberInfo(FieldInfo memberField)
    {
        Member = (TEnum)memberField.GetValue(null)!;
        MemberName = memberField.Name;
        Attributes = Attribute.GetCustomAttributes(memberField);



        var aliases = new Dictionary<string, string>();
        foreach (var attribute in Attributes)
        {
#if NET6_0_OR_GREATER
            if (attribute is DisplayAttribute displayAttribute)
            {
                addAlias("Display.Name", displayAttribute.Name);
                addAlias("Display.Description", displayAttribute.Description);
                addAlias("Display.ShortName", displayAttribute.ShortName);
                addAlias("Display", displayAttribute.Name ?? displayAttribute.Description ?? displayAttribute.ShortName);
            }
            else
#endif
            if (attribute is DescriptionAttribute descriptionAttribute)
            {
                addAlias("Description", descriptionAttribute.Description);
            }
            else if (attribute is EnumMemberAttribute enumMemberAttribute)
            {
                addAlias("EnumMember", enumMemberAttribute.Value);
            }
            else if (attribute is DataMemberAttribute dataMemberAttribute)
            {
                addAlias("DataMember", dataMemberAttribute.Name);
            }
        }

#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        aliases.TrimExcess();
#endif
#if NET8_0_OR_GREATER
        _aliases = aliases.ToFrozenDictionary();
#elif NET6_0_OR_GREATER
        _aliases = aliases.ToImmutableDictionary();
#else
        _aliases = new(aliases);
#endif

        return;

        void addAlias(string name, string? alias)
        {
            if (alias is not null)
            {
                aliases[name] = alias;
            }
        }
    }


}