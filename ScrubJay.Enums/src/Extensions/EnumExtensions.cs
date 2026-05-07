namespace ScrubJay.Enums.Extensions;

/// <summary>
/// Static extensions on <see cref="Enum"/>.
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    extension(Enum)
    {
        public static string Format<E>(E @enum, string? format)
            where E : struct, Enum
        {
            return ((IFormattable)@enum).ToString(format, null);
        }

        public static Type GetUnderlyingType<E>()
            where E : struct, Enum
        {
            return typeof(E).GetEnumUnderlyingType();
        }

#region TryParse -> Result
#region TryParse(Type)
        public static Result<Enum> TryParse(Type enumType,
            scoped text text, bool ignoreCase = true, bool includeAttributes = true)
        {
            if (enumType is null)
                return new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum)
                return new ArgumentException("Was not a valid Enum Type", nameof(enumType));

            return Enum.GetInfo(enumType)!
                .TryParseEnum(text, ignoreCase, includeAttributes);
        }

        public static Result<Enum> TryParse(Type enumType,
            string? str, bool ignoreCase = true, bool includeAttributes = true)
        {
            return Enum.TryGetInfo(enumType)
                .Select(info => info.TryParseEnum(str, ignoreCase, includeAttributes));
        }

        public static Result<Enum> TryParse(Type enumType,
            long i64)
        {
            return Enum.TryGetInfo(enumType)
                .Select(info => info.TryParseEnum(i64));
        }

        public static Result<Enum> TryParse(Type enumType,
            ulong u64)
        {
            return Enum.TryGetInfo(enumType)
                .Select(info => info.TryParseEnum(u64));
        }

        public static Result<Enum> TryParse(Type enumType,
            object? obj,
            bool ignoreCase = true,
            bool includeAttributes = true)
        {
            return Enum.TryGetInfo(enumType)
                .Select(info => info.TryParseEnum(obj, ignoreCase, includeAttributes));
        }
#endregion

#region TryParse<E>
        public static Result<E> TryParse<E>(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
            where E : struct, Enum
        {
            return Enum.GetInfo<E>().TryParse(text, ignoreCase, includeAttributes);
        }

        public static Result<E> TryParse<E>(string? str, bool ignoreCase = true, bool includeAttributes = true)
            where E : struct, Enum
        {
            return Enum.GetInfo<E>().TryParse(str, ignoreCase, includeAttributes);
        }

        public static Result<E> TryParse<E>(long i64)
            where E : struct, Enum
        {
            return Enum.GetInfo<E>().TryParse(i64);
        }

        public static Result<E> TryParse<E>(ulong u64)
            where E : struct, Enum
        {
            return Enum.GetInfo<E>().TryParse(u64);
        }

        public static Result<E> TryParse<E>(object? obj,
            bool ignoreCase = true,
            bool includeAttributes = true)
            where E : struct, Enum
        {
            return Enum.GetInfo<E>().TryParse(obj, ignoreCase, includeAttributes);
        }
#endregion
#endregion
    }
}