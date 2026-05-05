using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Exceptions;

/// <summary>
/// Utility class for creating <see cref="Exception">Exceptions</see>.
/// </summary>
[PublicAPI]
public static partial class Ex
{
    public static InvalidOperationException InvalidOperation(string? message = null, Exception? innerException = null)
    {
        return new InvalidOperationException(message, innerException);
    }

    public static NotImplementedException NotImplemented(
        string? message = null,
        Exception? innerException = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerFilePath] string? filePath = null,
        [CallerMemberName] string? memberName = null)
    {
        var caller = new CallerInfo(filePath, lineNumber, memberName);
        return new NotImplementedException(message, innerException);
    }
}