using System.Collections.Frozen;
using System.Globalization;
using ScrubJay.Extensions;

namespace ScrubJay.Testing.Things;

[PublicAPI]
public static class TestCharacters
{
    public static FrozenDictionary<UnicodeCategory, FrozenSet<char>> UnicodeCategorized { get; }

    public static class Ascii
    {
        public const string ALL =
            "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x20\x21\x22\x23\x24\x25\x26\x27\x28\x29\x2A\x2B\x2C\x2D\x2E\x2F\x30\x31\x32\x33\x34\x35\x36\x37\x38\x39\x3A\x3B\x3C\x3D\x3E\x3F\x40\x41\x42\x43\x44\x45\x46\x47\x48\x49\x4A\x4B\x4C\x4D\x4E\x4F\x50\x51\x52\x53\x54\x55\x56\x57\x58\x59\x5A\x5B\x5C\x5D\x5E\x5F\x60\x61\x62\x63\x64\x65\x66\x67\x68\x69\x6A\x6B\x6C\x6D\x6E\x6F\x70\x71\x72\x73\x74\x75\x76\x77\x78\x79\x7A\x7B\x7C\x7D\x7E\x7F";

        /// <summary>
        /// The ASCII control characters (0-31 + 127)
        /// </summary>
        /// <seealso href="https://www.ascii-code.com/characters/control-characters"/>
        public const string CONTROL = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x09\x0A\x0B\x0C\x0D\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x7F";

        /// <summary>
        /// The ASCII whitespace characters
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Code</term>
        ///         <description>Description</description>
        ///     </listheader>
        ///     <item>
        ///         <term>'\t' (0x09)</term>
        ///         <description>Horizontal Tab</description>
        ///     </item>
        ///     <item>
        ///         <term>'\n' (0x0A)</term>
        ///         <description>Line Feed</description>
        ///     </item>
        ///     <item>
        ///         <term>'\v' (0x0B)</term>
        ///         <description>Vertical Tab</description>
        ///     </item>
        ///     <item>
        ///         <term>'\f' (0x0C)</term>
        ///         <description>Form Feed</description>
        ///     </item>
        ///     <item>
        ///         <term>'\r' (0x0D)</term>
        ///         <description>Carriage Return</description>
        ///     </item>
        ///     <item>
        ///         <term>' ' (0x20)</term>
        ///         <description>Space</description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <seealso href="https://www.ascii-code.com/characters/white-space-characters"/>
        public const string WHITESPACE = "\t\n\v\f\r ";

        public const string CONTROL_NON_WHITESPACE = "\x00\x01\x02\x03\x04\x05\x06\x07\x08\x0E\x0F\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1A\x1B\x1C\x1D\x1E\x1F\x7F";

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://www.ascii-code.com/characters/printable-characters"/>
        public const string PRINTABLE = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

        /// <summary>
        /// 
        /// </summary>
        /// <seealso href="https://github.com/dotnet/csharpstandard/blob/standard-v6/standard/lexical-structure.md#6455-character-literals"/>
        public const string CSHARP_LITERALS = "\'\"\\\0\a\b\f\n\r\t\v";
    }


    static TestCharacters()
    {
        Dictionary<UnicodeCategory, List<char>> categorized = [];

        char ch;
        UnicodeCategory category;

        for (int i = ushort.MinValue; i <= ushort.MaxValue; i++)
        {
            ch = (char)i;

            category = char.GetUnicodeCategory(ch);
            categorized.GetOrAdd(category, []).Add(ch);
        }

        UnicodeCategorized = categorized.ToFrozenDictionary(
            static pair => pair.Key,
            static pair => pair.Value.ToFrozenSet());
    }
}