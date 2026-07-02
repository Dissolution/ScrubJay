//namespace ScrubJay.Errors;
//
///// <summary>
///// Utility class for creating <see cref="Exception">Exceptions</see>.
///// </summary>
//[PublicAPI]
//public static partial class Ex
//{
//    public static InvalidOperationException InvalidOperation(string? message = null, Exception? innerException = null)
//    {
//        return new InvalidOperationException(message, innerException);
//    }
//

//
//    public static NotSupportedException NotSupported(
//        string? info = null,
//        Exception? innerException = null,
//        [CallerLineNumber] int? lineNumber = null,
//        [CallerFilePath] string? filePath = null,
//        [CallerMemberName] string? memberName = null)
//    {
//        var caller = new CallerInfo(filePath, lineNumber, memberName);
//        var message = TextBuilder.Rent()
//            .Render(caller).Append(" is not supported")
//            .AppendInfo(info)
//            .ToStringAndDispose();
//        return new NotSupportedException(message, innerException);
//    }
//}