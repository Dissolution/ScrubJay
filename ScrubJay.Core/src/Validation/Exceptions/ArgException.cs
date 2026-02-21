using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Validation;

public static class ArgExceptionExtensions
{
    private static Func<Exception, string> _getMessage;

    static ArgExceptionExtensions()
    {
        typeof(Exception)
            .GetField("_message", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .ThrowIfNull($"Could not find {nameof(Exception)}.{"_message"} Field");
    }
    
    extension(Exception ex)
    {
        
    }
}

public class ArgException : ArgumentException
{
    public ArgException()
    {
    }

    public ArgException(string message) : base(message)
    {
    }

    public ArgException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ArgException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
    {
    }

    public ArgException(string message, string paramName) : base(message, paramName)
    {
    }

    public override string Message
    {
        get
        {
            var message = ((Exception)this).Message;
            return message;
        }
    }
    
    public override IDictionary Data { get; }
}