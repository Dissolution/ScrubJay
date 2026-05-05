namespace ScrubJay.Enums.Extensions;

/// <summary>
/// Extensions on <c>TEnum</c> instances <c>where TEnum : struct, Enum</c>
/// </summary>
[PublicAPI]
public static class TEnumInstanceExtensions
{
    // readonly extensions
    extension<E>(E @enum)
        where E : struct, Enum
    {
        public string ToString(EnumPart part, string? format = null)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(E other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(E other)
        {
            throw new NotImplementedException();
        }
    }
}