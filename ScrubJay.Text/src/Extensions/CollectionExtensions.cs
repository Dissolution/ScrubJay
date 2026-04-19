namespace ScrubJay.Text.Extensions;

internal static class CollectionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty<T>([AllowNull, NotNullWhen(false)] this T[]? array) => array is null || array.Length == 0;
}