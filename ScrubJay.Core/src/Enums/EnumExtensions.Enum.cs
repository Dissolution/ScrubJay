namespace ScrubJay.Enums;

partial class EnumExtensions
{
    extension(Enum)
    {
        public static Type GetUnderlyingType<E>()
            where E : struct, Enum
        {
            return typeof(E).GetEnumUnderlyingType();
        }
        
        [return: NotNullIfNotNull(nameof(@enum))]
        public static Type? GetUnderlyingType(Enum? @enum)
        {
            return @enum?.GetType().GetEnumUnderlyingType();
        }
    }

    extension(Enum? @enum)
    {
        public int ToInt32()
        {
            Throw.IfNull(@enum);
            return ((IConvertible)@enum).ToInt32(null);
        }

        public uint ToUInt32()
        {
            Throw.IfNull(@enum);
            return ((IConvertible)@enum).ToUInt32(null);
        }

        public long ToInt64()
        {
            Throw.IfNull(@enum);
            return ((IConvertible)@enum).ToInt64(null);
        }

        public ulong ToUInt64()
        {
            Throw.IfNull(@enum);
            return ((IConvertible)@enum).ToUInt64(null);
        }
    }
}