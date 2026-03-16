namespace ScrubJay.Enums;

/// <summary>
/// Extensions on <see cref="Enum"/> instances.
/// </summary>
[PublicAPI]
public static class EnumInstanceExtensions
{
    extension(Enum? @enum)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Enum? other)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(Enum? other)
        {
            throw new NotImplementedException();
        }
        
        public string? GetName()
        {
            throw new NotImplementedException();
        }

        public string Format(string? format = null)
        {
            throw new NotImplementedException();
        }

        public Result<T> TryConvertTo<T>()
        {
            throw new NotImplementedException();
        }
    }
}