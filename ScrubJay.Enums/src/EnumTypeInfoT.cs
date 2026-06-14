#pragma warning disable IDE0060

using System.Reflection;
using ScrubJay.Enums.Extensions;

namespace ScrubJay.Enums;

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
        return TryParse(text, ignoreCase, includeAttributes).Select(static e => (Enum)e);
    }

    public override Result<Enum> TryParseEnum(string? str, bool ignoreCase = true, bool includeAttributes = true)
    {
        return TryParse(str, ignoreCase, includeAttributes).Select(static e => (Enum)e);
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
        return TryParse(obj, ignoreCase, useAttributes).Select(static e => (Enum)e);
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
        return MemberInfos.FirstOrDefault(info => info.Member == @enum);
    }

    public override Option<EnumMemberInfo> TryGetMemberInfo(Enum? @enum)
    {
        if (@enum is TEnum t)
            return TryGetMemberInfo(t).Select(static e => (EnumMemberInfo)e);
        return None;
    }

    public Option<EnumMemberInfo<TEnum>> TryGetMemberInfo(TEnum @enum)
    {
        var info = MemberInfos.FirstOrDefault(info => info.Member == @enum);
        return Option.NotNull(info);
    }
}