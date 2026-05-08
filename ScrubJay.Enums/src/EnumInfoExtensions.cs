using System.Collections.Concurrent;
using System.Reflection;

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