namespace ScrubJay.Polyfills;

[PublicAPI]
public static class CharExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static text AsSpan(this in char ch)
    {
#if NET7_0_OR_GREATER
        return new text(in ch);
#else
        unsafe
        {
            return new text(Unsafe.InAsPtr<char>(in ch), 1);
        }
#endif
    }
}