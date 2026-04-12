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
            throw new NotImplementedException();
        }

        public static Result<Enum> TryParse(Type enumType,
            string? str, bool ignoreCase = true, bool includeAttributes = true)
        {
            throw new NotImplementedException();
        }

        public static Result<Enum> TryParse(Type enumType,
            long i64)
        {
            throw new NotImplementedException();
        }

        public static Result<Enum> TryParse(Type enumType,
            ulong u64)
        {
            throw new NotImplementedException();
        }

        public static Result<Enum> TryParse(Type enumType,
            object? obj,
            bool ignoreCase = true,
            bool useAttributes = true)
        {
            throw new NotImplementedException();
        }
#endregion

#region TryParse<E>
        public static Result<E> TryParse<E>(scoped text text, bool ignoreCase = true, bool includeAttributes = true)
            where E : struct, Enum
        {
            throw new NotImplementedException();
        }

        public static Result<E> TryParse<E>(string? str, bool ignoreCase = true, bool includeAttributes = true)
            where E : struct, Enum
        {
            throw new NotImplementedException();
        }

        public static Result<E> TryParse<E>(long i64)
            where E : struct, Enum
        {
            throw new NotImplementedException();
        }

        public static Result<E> TryParse<E>(ulong u64)
            where E : struct, Enum
        {
            throw new NotImplementedException();
        }

        public static Result<E> TryParse<E>(object? obj,
            bool ignoreCase = true,
            bool useAttributes = true)
            where E : struct, Enum
        {
            throw new NotImplementedException();
        }
#endregion
#endregion
    }
}