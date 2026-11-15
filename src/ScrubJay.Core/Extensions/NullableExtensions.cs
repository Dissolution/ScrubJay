namespace ScrubJay.Extensions;

/// <summary>
/// Extensions on <see cref="Nullable{T}"/> values.
/// </summary>
[PublicAPI]
public static class NullableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<T>([AllowNull, NotNullWhen(true)] this Nullable<T> nullable, [MaybeNullWhen(false)] out T value)
        where T : struct
    {
        if (nullable.HasValue)
        {
            value = nullable.GetValueOrDefault();
            return true;
        }
        
        value = default;
        return false;
    }
}