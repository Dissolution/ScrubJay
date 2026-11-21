namespace ScrubJay.Extensions;

[PublicAPI]
public static class StringExtensions
{
    extension(string)
    {
        public static int MaxLength => 0x3FFFFFDF;
    }

    extension([AllowNull, NotNullWhen(false)] string? str)
    {
        public bool IsNull => str is null;

        public bool IsEmpty => string.IsNullOrEmpty(str);

        public bool IsWhiteSpace => string.IsNullOrWhiteSpace(str);
    }

    extension([AllowNull, NotNullWhen(true)] string? str)
    {
        public bool IsNotNull => str is not null;

        public bool IsNotEmpty => !string.IsNullOrEmpty(str);

        public bool IsNotWhiteSpace => !string.IsNullOrWhiteSpace(str);
    }
}