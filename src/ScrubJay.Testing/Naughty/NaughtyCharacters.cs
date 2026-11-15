using ScrubJay.Testing.Things;

namespace ScrubJay.Testing.Naughty;

[PublicAPI]
public static class NaughtyCharacters
{
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



}