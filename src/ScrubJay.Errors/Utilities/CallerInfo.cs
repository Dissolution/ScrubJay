namespace ScrubJay.Errors.Utilities;

[PublicAPI]
[StructLayout(LayoutKind.Auto)]
public readonly record struct CallerInfo
{
    public static CallerInfo Capture(
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerMemberName] string? memberName = null)
    {
        return new(filePath, lineNumber, memberName);
    }

    public readonly string? FilePath;
    public readonly int? LineNumber;
    public readonly string? MemberName;

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
}