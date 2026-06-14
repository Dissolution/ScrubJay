namespace ScrubJay.Text.Building;

[PublicAPI]
public static class WhitespaceManager
{
    [JetBrains.Annotations.NotNull, AllowNull]
    public static string DefaultNewLine
    {
        get;
        set => field = value ?? Environment.NewLine;
    } = Environment.NewLine;

    [JetBrains.Annotations.NotNull, AllowNull]
    public static string DefaultIndent
    {
        get;
        set => field = value ?? "    ";
    } = "    ";
}