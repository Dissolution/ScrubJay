namespace ScrubJay.Text.Building;

[PublicAPI]
public static class WhitespaceManager
{
    [NotNull, AllowNull]
    public static string DefaultNewLine
    {
        get => field;
        set => field = value ?? Environment.NewLine;
    } = Environment.NewLine;

    [NotNull, AllowNull]
    public static string DefaultIndent
    {
        get => field;
        set => field = value ?? "    ";
    } = "    ";
}