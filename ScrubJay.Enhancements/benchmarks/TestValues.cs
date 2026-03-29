namespace ScrubJay.Text.Benchmarks;

[PublicAPI]
public static class TestValues
{
    // misc characters that are problems for various encodings
    public static readonly string SCARY_CHARS = "`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?";
    // https://github.com/dotnet/csharpstandard/blob/standard-v6/standard/lexical-structure.md#6455-character-literals
    public static readonly string CHAR_LITERALS = "\'\"\\\0\a\b\f\n\r\t\v";
    // https://en.wikipedia.org/wiki/Pangram
    public static readonly string PANGRAM = "Sphinx of black quartz, judge my vow!";

    public static readonly string SEED = CHAR_LITERALS + SCARY_CHARS + PANGRAM;

    public static string[] Strings { get; } =
    [
        string.Empty,
        ",",
        "\r\n",
        "Exception",
        PANGRAM,
        CreateTestString(128),
        CreateTestString(512),
        CreateTestString(4096),
    ];

    public static IEnumerable<object[]> StringsWithArrays()
    {
        foreach (var str in Strings)
        {
            char[] array = new char[str.Length + 16];
            yield return [str, array];
        }
    }

    private static string CreateTestString(int length)
    {
        Span<char> buffer = stackalloc char[length];
        for (int i = 0; i < length; i++)
        {
            buffer[i] = SEED[i % SEED.Length];
        }
        return buffer.ToString();
    }
}