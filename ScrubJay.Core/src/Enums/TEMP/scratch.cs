//using System.Globalization;
//using System.Reflection;
//using ScrubJay.Memory.Splitting;
//using ScrubJay.Parsing;
//
//namespace ScrubJay.Enums.TEMP;
//
//[PublicAPI]
//public static class EnumHelper
//{
//    extension(Enum)
//    {
//
//    }
//
//    extension(Enum? @enum)
//    {
//
//    }
//
//
//    extension<E>(E)
//        where E : struct, Enum
//    {
//
//    }
//
//    extension<E>(E @enum)
//        where E : struct, Enum
//    {
//
//    }
//
//
//    private static readonly ConcurrentTypeMap<EnumTypeInfo> _enumTypeInfos = [];
//
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private static EnumTypeInfo CreateEnumTypeInfo(Type enumType) => (Activator.CreateInstance(typeof(EnumTypeInfo<>).MakeGenericType(enumType)) as EnumTypeInfo)!;
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    private static EnumTypeInfo<E> CreateEnumTypeInfo<E>(Type enumType)
//        where E : struct, Enum
//        => Activator.CreateInstance<EnumTypeInfo<E>>();
//
//    public static EnumTypeInfo? GetEnumTypeInfo(Type? enumType)
//    {
//        if (enumType is null || !enumType.IsEnum)
//            return null;
//        return _enumTypeInfos.GetOrAdd(enumType, CreateEnumTypeInfo);
//    }
//
//    [return: NotNullIfNotNull(nameof(@enum))]
//    public static EnumTypeInfo? GetEnumTypeInfo(Enum? @enum)
//    {
//        if (@enum is null)
//            return null;
//        return _enumTypeInfos.GetOrAdd(@enum.GetType(), CreateEnumTypeInfo);
//    }
//
//    public static EnumTypeInfo<E> GetEnumTypeInfo<E>()
//        where E : struct, Enum
//    {
//        return (_enumTypeInfos.GetOrAdd<E>(CreateEnumTypeInfo) as EnumTypeInfo<E>)!;
//    }
//
//    public static EnumTypeInfo<E> GetEnumTypeInfo<E>(E @enum)
//        where E : struct, Enum
//    {
//        return (_enumTypeInfos.GetOrAdd<E>(CreateEnumTypeInfo) as EnumTypeInfo<E>)!;
//    }
//}
//
//public abstract class EnumTypeInfo
//{
//    protected readonly EnumMemberInfo[] _enumMemberInfos;
//
//    internal bool IsSigned { get; }
//
//    public Type EnumType { get; }
//
//    public Type UnderlyingType { get; }
//
//    public Attribute[] Attributes { get; }
//
//    public bool IsFlagged { get; }
//
//    public string Name => EnumType.Name;
//
//    public IReadOnlyList<EnumMemberInfo> Members => _enumMemberInfos;
//
//
//    protected EnumTypeInfo(Type enumType, EnumMemberInfo[] enumMemberInfos)
//    {
//        this.EnumType = enumType;
//        this.UnderlyingType = EnumType.GetEnumUnderlyingType()!;
//        this.IsSigned = Type.GetTypeCode(UnderlyingType) is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
//        this.Attributes = Attribute.GetCustomAttributes(EnumType);
//        this.IsFlagged = Attribute.GetCustomAttribute(enumType, typeof(FlagsAttribute)) is not null;
//        _enumMemberInfos = enumMemberInfos;
//    }
//}
//
//public sealed class EnumTypeInfo<E> : EnumTypeInfo
//    where E : struct, Enum
//{
//    private static EnumMemberInfo<E>[] GetMembers()
//    {
//        var memberFields = typeof(E).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
//        int count = memberFields.Length;
//
//        var members = new EnumMemberInfo<E>[count];
//        for (var i = 0; i < count; i++)
//        {
//            var memberField = memberFields[i];
//            var enumMemberInfo = (Activator.CreateInstance(typeof(EnumMemberInfo<E>), memberField) as EnumMemberInfo<E>)!;
//            members[i] = enumMemberInfo;
//        }
//        return members;
//    }
//
//
//    public new IReadOnlyList<EnumMemberInfo<E>> Members => (EnumMemberInfo<E>[])_enumMemberInfos;
//
//    public EnumTypeInfo() : base(typeof(E), (EnumMemberInfo[])GetMembers()) { }
//
//
//    public bool IsDefined(E @enum)
//    {
//        return Enum.IsDefined<E>(@enum);
//    }
//
//    public bool IsDefined(E @enum, [NotNullWhen(true)] out EnumMemberInfo<E>? enumMemberInfo)
//    {
//        if (Enum.IsDefined<E>(@enum))
//        {
//            enumMemberInfo = Members.First(m => m.Member == @enum);
//            return true;
//        }
//        else
//        {
//            enumMemberInfo = null;
//            return false;
//        }
//    }
//
//    private Result<E> TryParseOne(scoped text text)
//    {
//        foreach (var enumMember in Members)
//        {
//#if NET9_0_OR_GREATER
//            var lookup = enumMember._aliases.GetAlternateLookup<text>();
//            if (lookup.Contains(text))
//            {
//                return enumMember.Member;
//            }
//#else
//                foreach (var alias in enumMember._aliases)
//                {
//                    if (text.Equate(alias))
//                    {
//                        return enumMember.Member;
//                    }
//                }
//#endif
//        }
//
//        if (int.TryParse(text, NumberStyles.Any, null, out int i32))
//        {
//            foreach (var enumMember in Members)
//            {
//                if (enumMember.Int32Value == i32)
//                {
//                    return enumMember.Member;
//                }
//            }
//        }
//
//        // could not
//        return Ex.Parse<E>(text);
//    }
//
//    private Result<E> TryParseOne(string str)
//    {
//        foreach (var enumMember in Members)
//        {
//            if (enumMember._aliases.Contains(str))
//            {
//                return enumMember.Member;
//            }
//        }
//
//        if (int.TryParse(str, NumberStyles.Any, null, out int i32))
//        {
//            foreach (var enumMember in Members)
//            {
//                if (enumMember.Int32Value == i32)
//                {
//                    return enumMember.Member;
//                }
//            }
//        }
//
//        // could not
//        return Ex.Parse<E>(str);
//    }
//
//    public Result<E> TryParse(scoped text text)
//    {
//        if (IsFlagged)
//        {
//            E @enum = default;
//            var segments = text.SplitAny(",|", SplitOptions.IgnoreEmpty | SplitOptions.Trim);
//            if (!segments.TryMoveNext(out var segment))
//            {
//                return Ex.Parse<E>(text);
//            }
//
//            do
//            {
//                if (TryParseOne(segment).IsError(out var ex, out var flag))
//                    return ex;
//
//                @enum.AddFlag(flag);
//            } while (segments.TryMoveNext(out segment));
//
//            return @enum;
//        }
//        else
//        {
//            return TryParseOne(text);
//        }
//    }
//
//    public Result<E> TryParse(string? str)
//    {
//        if (str is null)
//            return Ex.ArgNull<string>(nameof(str));
//
//        if (IsFlagged)
//        {
//            E @enum = default;
//            var segments = str.SplitAny(",|", SplitOptions.IgnoreEmpty | SplitOptions.Trim);
//            if (!segments.TryMoveNext(out var segment))
//            {
//                return Ex.Parse<E>(str);
//            }
//
//            do
//            {
//                if (TryParseOne(segment).IsError(out var ex, out var flag))
//                    return ex;
//
//                @enum.AddFlag(flag);
//            } while (segments.TryMoveNext(out segment));
//
//            return @enum;
//        }
//        else
//        {
//            return TryParseOne(str);
//        }
//    }
//}
//
