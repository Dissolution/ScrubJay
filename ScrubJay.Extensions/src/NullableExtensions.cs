namespace ScrubJay.Extensions;

[PublicAPI]
public static class NullableExtensions
{
    extension<T>(Nullable<T> nullable)
        where T : struct
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsNotNull(out T value)
        {
            value = nullable.GetValueOrDefault();
            return nullable.HasValue;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, MaybeNull] this T value,
        [NotNullWhen(true), MaybeNullWhen(false)] out T nonNullValue)
    {
        nonNullValue = value!;
        return value is not null;
    }
}