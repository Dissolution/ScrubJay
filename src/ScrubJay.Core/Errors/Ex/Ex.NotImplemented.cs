using ScrubJay.Debugging.Logging;

namespace ScrubJay.Errors;

public static partial class Ex
{
    [StackTraceHidden]
    public static NotImplementedException NotImplemented(
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerMemberName] string? memberName = null)
    {
        var caller = new CallerInfo(filePath, lineNumber, memberName);
        var message = CreateMessage($"{caller} has not yet been implemented", additionalInfo);
        return new NotImplementedException(message, innerException);
    }
}