namespace ScrubJay.Enums;

/// <summary>
/// Static extensions on <see cref="Enum"/>.
/// </summary>
[PublicAPI]
public static class EnumExtensions
{
    extension(Enum)
    {
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
    }
}