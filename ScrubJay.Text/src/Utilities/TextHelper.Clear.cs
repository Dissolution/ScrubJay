namespace ScrubJay.Text.Utilities;

public static partial class TextHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(scoped Span<char> chars)
    {
        unsafe
        {
            fixed (char* ptr = chars)
            {
                Unsafe.InitCharBlock(ptr, chars.Length);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(char[]? chars)
    {
        if (chars is not null)
        {
            unsafe
            {
                fixed (char* ptr = chars)
                {
                    Unsafe.InitCharBlock(ptr, chars.Length);
                }
            }
        }
    }
}