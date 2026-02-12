#pragma warning disable CA1819

using System.Reflection;


namespace ScrubJay.Enums;

[PublicAPI]
public abstract class EnumTypeInfo :
#if NET7_0_OR_GREATER
    IEqualityOperators<EnumTypeInfo, EnumTypeInfo, bool>,
#endif
    IEquatable<EnumTypeInfo>
{
    public static bool operator ==(EnumTypeInfo? left, EnumTypeInfo? right)
        => Equate.Values(left, right);

    public static bool operator !=(EnumTypeInfo? left, EnumTypeInfo? right)
        => !Equate.Values(left, right);


    protected static readonly ConcurrentTypeMap<EnumTypeInfo> _cache = [];

    protected static EnumTypeInfo CreateEnumInfo(Type enumType)
    {
        var enumInfo = typeof(EnumTypeInfo<>)
            .MakeGenericType(enumType)
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .FirstOrDefault()
            .ThrowIfNull()
            .Invoke(parameters: null)
            .ThrowIfNot<EnumTypeInfo>();
        return enumInfo;
    }

    public static EnumTypeInfo For(Type enumType)
    {
        Guard.IsNotNull(enumType);
        Guard.IsEnum(enumType);
        return _cache.GetOrAdd(enumType, CreateEnumInfo);
    }

    public static EnumTypeInfo For(Enum @enum)
    {
        Throw.IfNull(@enum);
        return For(@enum.GetType());
    }

    public static EnumTypeInfo<E> For<E>(E _ = default)
        where E : struct, Enum
    {
        return _cache.GetOrAdd<E>(CreateEnumInfo).ThrowIfNot<EnumTypeInfo<E>>();
    }

    protected readonly EnumMemberInfo[] _members;

    public Attribute[] Attributes { get; }
    public Type EnumType { get; }
    public Type UnderlyingType { get; }
    public bool IsFlags => Attributes.Contains<FlagsAttribute>();
    public IEnumerable<EnumMemberInfo> Members => _members;

    protected EnumTypeInfo(Type enumType)
    {
        Debug.Assert(enumType is not null);
        Debug.Assert(enumType!.IsValueType);
        Debug.Assert(enumType.IsEnum);

        EnumType = enumType;
        UnderlyingType = Enum.GetUnderlyingType(enumType);
        Attributes = Attribute.GetCustomAttributes(enumType);

        var members = EnumType.GetFields(BindingFlags.Public | BindingFlags.Static);
        _members = new EnumMemberInfo[members.Length];
        for (var i = 0; i < members.Length; i++)
        {
            // get this field
            var memberField = members[i];
            // construct an EnumMemberInfo<E> with it
            var enumMemberInfo = typeof(EnumMemberInfo<>)
                .MakeGenericType(enumType)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault()
                .ThrowIfNull()
                .Invoke(parameters: [this, memberField])
                .ThrowIfNot<EnumMemberInfo>();
            _members[i] = enumMemberInfo;
        }
    }

    public Option<EnumMemberInfo> TryGetMemberInfo(string? name)
    {
        return _members
            .FirstOrDefault(m => m.HasAlias(name))
            .IsNotNull();
    }

    public Option<EnumMemberInfo> TryGetMemberInfo(long value)
    {
        return _members.FirstOrDefault(m => m.I64Value == value).IsNotNull();
    }

    public Option<EnumMemberInfo> TryGetMemberInfo(Enum? mune)
    {
        if (mune is null)
            return None;

        if (mune.GetType() != EnumType)
            return None;

        long value = ((IConvertible)mune).ToInt64(null);

        return _members
            .FirstOrDefault(m => m.I64Value == value)
            .IsNotNull();
    }
    
    public bool Equals(EnumTypeInfo? enumInfo)
    {
        return enumInfo is not null && enumInfo.EnumType == EnumType;
    }

    public override bool Equals(object? obj)
    {
        if (obj is EnumTypeInfo enumInfo)
            return Equals(enumInfo);
        if (obj is Type enumType)
            return enumType == EnumType;
        return false;
    }

    public override int GetHashCode()
    {
        return Hasher.Hash(EnumType);
    }

    public override string ToString()
    {
        return TypeName.For(EnumType);
    }
}