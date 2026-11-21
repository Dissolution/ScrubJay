#pragma warning disable CA5394
using ScrubJay.Extensions;

namespace ScrubJay.Testing.Things;

[PublicAPI]
public static class TestStrings
{
    // https://en.wikipedia.org/wiki/Pangram
    public static readonly IReadOnlyList<string> Pangrams =
    [
        "Quick nymph bugs vex fjord waltz.",
        "Waltz, bad nymph, for quick jigs vex.",
        "Glib jocks quiz nymph to vex dwarf.",
        "Sphinx of black quartz, judge my vow.",
        "How quickly daft jumping zebras vex!",
        "The five boxing wizards jump quickly.",
        "Jackdaws love my big sphinx of quartz.",
        "Pack my box with five dozen liquor jugs.",
        "The quick brown fox jumps over the lazy dog.",
    ];

    public static string Word(int minLength = 1, int maxLength = 64)
    {
        int length = Random.Shared.Next(minLength, maxLength + 1);
        Span<char> buffer = length <= 1024 ? stackalloc char[length] : new char[length];

        buffer[0] = (char)(Random.Shared.Next(26) + 'A');
        for (var i = 1; i < length; i++)
        {
            buffer[i] = (char)(Random.Shared.Next(26) + 'a');
        }

        return new string(buffer);
    }

    public static string Create(text charset, int length)
    {
        if (charset.IsEmpty)
            throw new ArgumentOutOfRangeException(nameof(charset), charset.ToString(), "Must have at least one character");
        if (length < 0 || length >= string.MaxLength)
            throw new ArgumentOutOfRangeException(nameof(length), length, "Length must be in ");

        Span<char> buffer = length <= 1024 ? stackalloc char[length] : new char[length];
        int max = charset.Length;

        for (var i = 0; i < buffer.Length; i++)
        {
            int r = Random.Shared.Next(max);
            buffer[i] = charset[r];
        }

        return new string(buffer);
    }

    public static string Create(IReadOnlyList<char> charset, int length)
    {
        if (charset.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(charset), charset.ToString(), "Must have at least one character");
        if (length < 0 || length >= string.MaxLength)
            throw new ArgumentOutOfRangeException(nameof(length), length, "Length must be in ");

        Span<char> buffer = length <= 1024 ? stackalloc char[length] : new char[length];
        int max = charset.Count;

        for (var i = 0; i < buffer.Length; i++)
        {
            int r = Random.Shared.Next(max);
            buffer[i] = charset[r];
        }

        return new string(buffer);
    }
}