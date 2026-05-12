#pragma warning disable CA5394

using System.Reflection;
using Xunit.Internal;

#if NET8_0_OR_GREATER
using System.Security.Cryptography;
#endif

namespace ScrubJay.Universal.Tests.Internal;

internal static partial class TestTypes
{
    public static object?[] Objects { get; } =
    [
        // null
        (object?)null,
        // valid flags enum
        BindingFlags.Public | BindingFlags.NonPublic,
        // invalid non-flags enum
        (StringComparison)147,
        // primitive
        (byte)147,
        -0.0d,
        // custom primitive
        default(NoToString.TestStruct),
        // struct
        IntPtr.Zero,
        // Nullable
        (Nullable<int>)null,
        (Nullable<int>)147,
        // char
        '❤',
        "\uD800",
        // string
        (string?)null,
        string.Empty,
        "Sphinx of black quartz, judge my vow!",
        "🏃🏻‍➡️ 🟫 🦊 🦘 🆙 🦥 🐕",
        "pass\0word",
        // delegate
        new Action(static () => { }),
        // object itself
        new object(),
        // type
        typeof(TestTypes),
        typeof(IList<>),
        // exception
        new Exception(nameof(TestTypes)),
        // old net stuff
        DBNull.Value,
        // anonymous object
        new { Id = 147, Name = "TJ", IsAdmin = true, },
        // array
        new byte[4] { 0, 147, 13, 101 },
        Array.Empty<object?>(),
        // simple dictionary
        new Dictionary<int, string>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
        },
        // complex class
        AppDomain.CurrentDomain,
        Enumerable.Range(0,13),
        Task.FromException(new InvalidOperationException()),
    ];
}

internal static partial class TestTypes
{
    [PublicAPI]
    public static class Text
    {
        public const string CONTROL_BLOCK = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x7F";
        public const string BAD_CSV = "\",\n\r\0\t";
        public const string BAD_JSON = "\"\\/\n\r\t\b\f\0";
        public const string BAD_XML = "<>&\"'\0\t\n\r";
        public const string BAD_TOML = "\"\\\n\r\t\b\f\0'#=";
        public const string BAD_YAML = ":#\";\\\n\0\t[]{},>|!&*%@`";
        public const string BAD_SHELL = "$`!\\\"'~?*[]{}()<>|&;#\n";
        public const string BAD_CMD = "%^&|<>\"!()\n";
        public const string BAD_POWERSHELL = "`$\"'#@(){}|&<>[]";
        public const string BAD_REGEX = ".*+?^$\\|()[]{}";
        public const string BAD_INI = "=:#;[]\\\n";
        public const string BAD_URL = " #%&+=?/:@[]";
        public const string BAD_MARKDOWN = "\\*_`#[]()!>-+|~";
        public const string BAD_SQL = "'\\\"%_[]`\0\n\r";
        public const string BAD_MIME = "\r\n:\"\\()";
        public const string BAD_WIN_PATH = "\\/:*?\"<>|";
        // https://github.com/dotnet/csharpstandard/blob/standard-v6/standard/lexical-structure.md#6455-character-literals
        public const string CSHARP_LITERALS = "\'\"\\\0\a\b\f\n\r\t\v";

        // https://en.wikipedia.org/wiki/Pangram
        private static readonly string[] PANGRAMS =
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

        public static IReadOnlyCollection<char> ScaryCharacters { get; }

        public static char[] Characters { get; } =
        [
            char.MinValue,
            ' ',
            (char)128,
            char.MaxValue,
        ];


        public static string[] Strings { get; }
        public static string?[] StringsAndNull { get; }


        public static string SEED { get; }

        static Text()
        {
            var scaryChars = new HashSet<char>();
            scaryChars.AddRange("\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x7F");
            scaryChars.AddRange("\",\n\r\0\t");
            scaryChars.AddRange("\"\\/\n\r\t\b\f\0");
            scaryChars.AddRange("<>&\"'\0\t\n\r");
            scaryChars.AddRange("\"\\\n\r\t\b\f\0'#=");
            scaryChars.AddRange(":#\";\\\n\0\t[]{},>|!&*%@`");
            scaryChars.AddRange("$`!\\\"'~?*[]{}()<>|&;#\n");
            scaryChars.AddRange("%^&|<>\"!()\n");
            scaryChars.AddRange("`$\"'#@(){}|&<>[]");
            scaryChars.AddRange(".*+?^$\\|()[]{}");
            scaryChars.AddRange("=:#;[]\\\n");
            scaryChars.AddRange(" #%&+=?/:@[]");
            scaryChars.AddRange("\\*_`#[]()!>-+|~");
            scaryChars.AddRange("'\\\"%_[]`\0\n\r");
            scaryChars.AddRange("\r\n:\"\\()");
            scaryChars.AddRange("\\/:*?\"<>|");
            scaryChars.AddRange("\'\"\\\0\a\b\f\n\r\t\v");
            ScaryCharacters = scaryChars;

#if NET8_0_OR_GREATER
            var builder = new StringBuilder()
                .AppendJoin(null, scaryChars)
                .AppendJoin(null, PANGRAMS);
            var seed = new char[builder.Length];
            builder.CopyTo(0, seed, 0, builder.Length);
            RandomNumberGenerator.Shuffle<char>(seed);
            SEED = new string(seed);
#else
            var builder = new StringBuilder();
            foreach (var ch in scaryChars)
            {
                builder.Append(ch);
            }
            foreach (var p in PANGRAMS)
            {
                builder.Append(p);
            }
            var len = builder.Length;
            var seed = new char[builder.Length];
            builder.CopyTo(0, seed, 0, len);
            // Fisher-Yates
            var rand = new Random();
            for (int i = 0; i < len - 1; i++)
            {
                int j = rand.Next(i, len);

                if (i != j)
                {
                    (seed[i], seed[j]) = (seed[j], seed[i]);
                }
            }

            SEED = new string(seed);
#endif
            Strings =
            [
                string.Empty,
                ",",
                "\r\n",
                "Exception",
                PANGRAMS[3],
                CreateTestString(128),
                CreateTestString(512),
                CreateTestString(4096),
            ];

            StringsAndNull =
            [
                string.Empty,
                ",",
                "\r\n",
                "Exception",
                PANGRAMS[3],
                CreateTestString(128),
                CreateTestString(512),
                CreateTestString(4096),
                null,
            ];

        }

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
}