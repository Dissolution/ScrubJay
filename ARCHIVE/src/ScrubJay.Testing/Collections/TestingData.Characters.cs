using System.Globalization;

namespace ScrubJay.Testing.Collections;

partial class TestingData
{
        [PublicAPI]
    public static class Characters
    {
        public const string CONTROL_BLOCK = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x7F";
        // https://github.com/dotnet/csharpstandard/blob/standard-v6/standard/lexical-structure.md#6455-character-literals
        public const string CSHARP_LITERALS = "\'\"\\\0\a\b\f\n\r\t\v";

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

        public static IReadOnlyCollection<char> ScaryCharacters { get; }

        public static IReadOnlyDictionary<UnicodeCategory, IReadOnlyList<char>> CategorizedCharacters { get; }

        static Characters()
        {
            var scaryChars = new HashSet<char>();
            scaryChars.AddRange(CONTROL_BLOCK);
            scaryChars.AddRange(CSHARP_LITERALS);
            scaryChars.AddRange(BAD_CSV);
            scaryChars.AddRange(BAD_JSON);
            scaryChars.AddRange(BAD_XML);
            scaryChars.AddRange(BAD_TOML);
            scaryChars.AddRange(BAD_YAML);
            scaryChars.AddRange(BAD_SHELL);
            scaryChars.AddRange(BAD_CMD);
            scaryChars.AddRange(BAD_POWERSHELL);
            scaryChars.AddRange(BAD_REGEX);
            scaryChars.AddRange(BAD_INI);
            scaryChars.AddRange(BAD_URL);
            scaryChars.AddRange(BAD_MARKDOWN);
            scaryChars.AddRange(BAD_SQL);
            scaryChars.AddRange(BAD_MIME);
            scaryChars.AddRange(BAD_WIN_PATH);
            ScaryCharacters = scaryChars;

            char ch;
            UnicodeCategory category;

            Dictionary<UnicodeCategory, List<char>> categorized = new();

            for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
            {
                ch = (char)i;
                category = char.GetUnicodeCategory(ch);

                categorized.GetOrAdd(category, []).Add(ch);
            }

            CategorizedCharacters = categorized
                .ToDictionary(pair => pair.Key, pair =>
                {
                    var val = pair.Value;
                    val.TrimExcess();
                    return (IReadOnlyList<char>)val;
                });
        }
    }
}