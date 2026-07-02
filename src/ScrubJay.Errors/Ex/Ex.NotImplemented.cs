using ScrubJay.Errors.Utilities;

namespace ScrubJay.Errors;

partial class Ex
{
    public static NotImplementedException NotImplemented(
        string? info = null,
        Exception? innerException = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerMemberName] string? memberName = null)
    {
        var caller = new CallerInfo(filePath, lineNumber, memberName);
        DefaultInterpolatedStringHandler builder = $"{caller} has not yet been implemented";
        if (!string.IsNullOrEmpty(info))
        {
            builder.AppendLiteral(" - ");
            builder.AppendLiteral(info!);
        }
        string message = builder.ToStringAndClear();
        return new NotImplementedException(message, innerException);
    }
}