namespace ScrubJay.Extensions;

[PublicAPI]
public static class StringExtensions
{
    extension(string)
    {
        public static int MaxLength => 0x3FFFFFDF;
    }
}