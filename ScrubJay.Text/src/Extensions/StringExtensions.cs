namespace ScrubJay.Text.Extensions;

public static class StringExtensions
{
    extension(string)
    {
        public static int MaxLength
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 0x3FFFFFDF;
        }
    }
}