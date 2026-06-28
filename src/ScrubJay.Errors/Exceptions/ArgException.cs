using ScrubJay.Errors.Arguments;
using ScrubJay.Errors.Utilities;

namespace ScrubJay.Errors.Exceptions;

/// <summary>
/// An enhanced <see cref="ArgumentException"/>.
/// </summary>
[PublicAPI]
public sealed class ArgException : ArgumentException, IScrubJayException<ArgException>
{
    public ArgumentInfo ArgumentInfo { get; }

    public override string? Message => ExceptionAccess.RefParamNameField(this);

    public ArgException(
        ArgumentInfo argument,
        string? message = null,
        Exception? innerException = null)
        : base(message, argument.Name, innerException)
    {
        ArgumentInfo = argument;
    }

    public override string ToString()
    {
        
    }
}