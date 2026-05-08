namespace ScrubJay.Text.Extensions;

internal static class ArrayExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNull<T>([AllowNull, NotNullWhen(false)] this T[]? array) => array is null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrEmpty<T>([AllowNull, NotNullWhen(false)] this T[]? array) => array is null || array.Length == 0;
}