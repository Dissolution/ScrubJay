namespace ScrubJay.Extensions;

[PublicAPI]
public static class CharExtensions
{
    extension(char)
    {
        public static bool IsEndOfSentencePunctuation(char ch)
        {
            return ch is '.' or '!' or '?' or '…' or '‽';
        }
    }

    extension(in char ch)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public text AsSpan() => new(in ch);
    }
}