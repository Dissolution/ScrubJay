using System.ComponentModel;
using System.Runtime.Serialization;

namespace ScrubJay.Errors.Exceptions;

public sealed class InvalidEnumException : InvalidEnumArgumentException
{
    public InvalidEnumException()
    {
    }

    public InvalidEnumException(string message) : base(message)
    {
    }

    public InvalidEnumException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public InvalidEnumException(string argumentName, int invalidValue, Type enumClass) : base(argumentName, invalidValue, enumClass)
    {
    }
}