namespace ScrubJay.Errors.Utilities;

[PublicAPI]
public sealed record class CallerInfo : IRenderable
{
    public static CallerInfo Capture(
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerMemberName] string? memberName = null)
    {
        return new(filePath, lineNumber, memberName);
    }

    public string? FilePath { get; init; }
    public int? LineNumber { get; init; }
    public string? MemberName { get; init; }

    public CallerInfo()
    {
    }

    public CallerInfo(string? filePath, int? lineNumber, string? memberName)
    {
        FilePath = filePath;
        LineNumber = lineNumber;
        MemberName = memberName;
    }

    public override string ToString()
    {
        return $"{FilePath}:{LineNumber} - {MemberName}";
    }

    public void RenderTo(TextBuilder builder)
    {
        builder.IfNotEmpty(FilePath, TB.Append, TB.Write("????.???"))
            .Append(':')
            .IfNotNull(LineNumber, TB.Append<int>, TB.Write('?'))
            .Append(" - ")
            .IfNotEmpty(MemberName, TB.Append, TB.Write("??"));
    }
}