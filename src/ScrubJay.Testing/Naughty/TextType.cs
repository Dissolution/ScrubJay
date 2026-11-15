namespace ScrubJay.Testing.Naughty;

[PublicAPI]
[Flags]
public enum TextType
{
    None = 0,
    AsciiControl = 1 << 0,
    Csharp = 1 << 1,
    CSV = 1 << 2,
    JSON = 1 << 3,
    XML = 1 << 4,
    TOML = 1 << 5,
    YAML = 1 << 6,
    SHELL = 1 << 7,
    CMD = 1 << 8,
    POWERSHELL = 1 << 9,
    REGEX = 1 << 10,
    INI = 1 << 11,
    URL = 1 << 12,
    MARKDOWN = 1 << 13,
    SQL = 1 << 14,
    MIME = 1 << 15,
    WIN_PATH = 1 << 16,
}