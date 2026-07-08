namespace ScrubJay.Testing.Collections;

partial class TestingData
{
    [PublicAPI]
    public static class Strings
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

        public static IReadOnlyList<string> Whitespace { get; } =
        [
            "",
            " ",
            "\t",
            "\r",
            "\n",
        ];

        private static string GetStringImpl(int length) => string.Create(length, Random.Shared, static (span, random) =>
        {
            for (var i = 0; i < span.Length; i++)
            {
                // printable ASCII
                char ch = (char)random.Next(minValue: 32, maxValue: 127);
                span[i] = ch;
            }
        });

        public static string GetString(int minLength = 0, int maxLength = 1025)
        {
            minLength = Math.Clamp(minLength, 0, 64_000);
            maxLength = Math.Clamp(maxLength, 0, 64_000);

            int range = maxLength - minLength;
            if (range <= 0)
            {
                return string.Empty;
            }

            var len = Random.Shared.Next(minLength, maxLength);
            return GetStringImpl(len);
        }

        public static IReadOnlyList<string> GetStrings(int count, int minLength = 0, int maxLength = 1025)
        {
            if (count <= 0)
                return [];

            minLength = Math.Clamp(minLength, 0, 64_000);
            maxLength = Math.Clamp(maxLength, 0, 64_000);

            string[] strings = new string[count];

            int range = maxLength - minLength;
            if (range <= 0)
            {
                strings.SetAll("");
                return strings;
            }

            if (count == 1)
            {
                strings[0] = GetStringImpl(range / 2);
                return strings;
            }

            var step = (int)((float)range) / (count - 1);
            int s = 0;

            for (int len = 0; len < maxLength; len += step, s++)
            {
                strings[s] = GetStringImpl(len);
            }

            if (s != strings.Length)
                Debugger.Break();

            return strings;
        }

        public static IReadOnlyList<string?> GetStringsAndNull(int count, int minLength = 0, int maxLength = 1025)
        {
            if (count <= 0)
                return [];

            minLength = Math.Clamp(minLength, 0, 64_000);
            maxLength = Math.Clamp(maxLength, 0, 64_000);

            string?[] strings = new string?[count];

            int range = maxLength - minLength;
            if (range <= 0)
            {
                strings.SetAll("");
                return strings;
            }

            strings[0] = null;

            if (count == 1)
                return strings;
            
            var step = (int)((float)range) / (count - 2);
            int s = 1;

            for (int len = 0; len < maxLength; len += step, s++)
            {
                strings[s] = GetStringImpl(len);
            }

            if (s != strings.Length)
                Debugger.Break();

            return strings;
        }


    }

}