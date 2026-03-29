namespace ScrubJay.Enhancements.Text.Utilities;

public static partial class TextHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryCopyTo(ReadOnlySpan<char> source, Span<char> dest)
    {
        int len = source.Length;
        if (len <= dest.Length)
        {
            Notsafe.CopyText(source, dest, len);
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
            Notsafe.CopyText(source, dest, len);
            return true;
        }
        return false;
    }
}