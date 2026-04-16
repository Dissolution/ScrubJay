namespace ScrubJay.Text.Utilities;

public static partial class TextHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCopyTo(ReadOnlySpan<char> source, Span<char> dest)
    {
        int len = source.Length;
        if (len <= dest.Length)
        {
            Unsafe.CopyTo(source, dest, len);
            return true;
        }
        return false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCopyTo(string source, Span<char> dest)
    {
        int len = source.Length;
        if (len <= dest.Length)
        {
            Unsafe.CopyTo(source, dest, len);
            return true;
        }
        return false;
    }
}