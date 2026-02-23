#if !(NETFRAMEWORK || NETSTANDARD)
using System.ComponentModel.DataAnnotations;
#endif
using ScrubJay.Rendering.Rendition5;

namespace ScrubJay.Enums;

public static class EnumHelper
{
    private static readonly ConcurrentTypeMap<EnumInfo> _cache = [];

    private static EnumInfo CreateEnumInfo(Type enumType)
    {
        return Activator
            .CreateInstance(typeof(EnumInfo<>).MakeGenericType(enumType))
            .ThrowIfNot<EnumInfo>();
    }

    public static EnumInfo? GetEnumInfo(Type? enumType)
    {
        if (enumType is null)
            return null;
        if (!enumType.IsEnum)
            return null;
        return _cache.GetOrAdd(enumType, CreateEnumInfo);
    }

    public static EnumInfo<E> GetEnumInfo<E>()
        where E : struct, Enum
    {
        return _cache.GetOrAdd<E>(CreateEnumInfo).ThrowIfNot<EnumInfo<E>>();
    }

    public static Result<E> TryParse<E>(string? str)
        where E : struct, Enum
    {
        var info = GetEnumInfo<E>();
        return info.TryParse(str);
    }



    [RenderToMethod]
    public static void RenderEnumTo<E>(E @enum, TextBuilder builder)
        where E : struct, Enum
    {
        EnumInfo<E> enumInfo = GetEnumInfo<E>();
        if (enumInfo.IsFlags)
        {
            builder.Delimit(" | ", @enum.EnumerateFlags(), (tb, flag) =>
            {
                tb.Write(enumInfo.GetMember(flag)?.Rendered);
            });
        }
        else
        {
            builder.Write(enumInfo.GetMember(@enum)?.Rendered);
        }
    }

}