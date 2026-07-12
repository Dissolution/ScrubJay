namespace ScrubJay.Text.Extensions;

[PublicAPI]
public static class CharExtensions
{
    extension(ref readonly char ch)
    {
        public text AsSpan()
        {
#if NET7_0_OR_GREATER
            return new text(in ch);
#else
            unsafe
            {
                return new text(Unsafe.AsPointer<char>(in ch), 1);
            }
#endif
        }
    }
}