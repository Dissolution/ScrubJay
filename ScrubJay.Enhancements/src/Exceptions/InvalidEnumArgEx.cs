using System.ComponentModel;

namespace ScrubJay.Enhancements.Exceptions;

/// <summary>
/// 
/// </summary>
/// <seealso href="https://github.com/dotnet/runtime/issues/34983"/>
public class InvalidEnumArgEx : InvalidEnumArgumentException, IEnhancedException
{
    public new string Message
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public InvalidEnumArgEx(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public InvalidEnumArgEx(string? argumentName, int invalidValue, Type enumClass) : base(argumentName, invalidValue, enumClass)
    {

    }
}