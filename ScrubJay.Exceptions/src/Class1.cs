using System.Collections.Specialized;
using System.ComponentModel;
using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

internal static class ScratchPad
{
    static ScratchPad()
    {

        
    }
}

[PublicAPI]
public sealed record class Argument(string? Name, Type? Type, object? Value)
{
    public static Argument Create(string? name, object? value)
        => new(name, value?.GetType(), value);
    
    public static Argument Create<T>(string? name, T? value)
        => new(name, Any.GetType(in value), (object?)value);
    
    public static Argument Create(string? name, Type? type, object? value)
        => new(name, type, value);

    public static Argument Create<T>(T? value, [CallerArgumentExpression(nameof(value))] string? argumentName = null)
        => new(argumentName, Any.GetType(in value), (object?)value);
    
    public static Argument Create(object? value, [CallerArgumentExpression(nameof(value))] string? argumentName = null)
        => new(argumentName, value?.GetType(), (object?)value);


    public bool ContainsNull => Value is null;
}

[PublicAPI]
public interface IEnhancedException
{
    
}

[PublicAPI]
public interface IEnhancedArgumentException : IEnhancedException
{
    Argument Argument { get; }
}

public static class EnhancedExceptionExtensions
{
    extension<E>(E ex)
        where E : Exception, IEnhancedException
    {
        public Uri HResultInfo => new HResult(ex.HResult).InfoUri;

        internal ref string? RefMessageField()
        {
            return ref RefExceptionMessageField(ex);
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    internal extern static ref string? RefExceptionMessageField(Exception exception);

    internal static StringBuilder RenderArgument(this StringBuilder builder, Argument argument)
    {
        builder.Append("Argument (")
            .Append(argument.Name ?? "<?>")
            .Append(": ");
        if (argument.Type is null)
        {
            if (argument.Value is null)
            {
                builder.Append("null");
            }
            else
            {
                builder.RenderType(argument.Value.GetType());
            }
        }
        else
        {
            builder.RenderType(argument.Type);
        }
        return builder.Append(" = ").Append(argument.Value).Append(')');
    }
}

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

public class NullArgEx : ArgumentNullException, IEnhancedException
{
    public NullArgEx(string? paramName) : base(paramName)
    {
    }

    public NullArgEx(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public NullArgEx(string? paramName, string? message) : base(paramName, message)
    {
    }
}

public class OutOfRangeArgEx : ArgumentOutOfRangeException, IEnhancedException
{
    public override object? ActualValue { get; }
 
    public OutOfRangeArgEx(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public OutOfRangeArgEx(string? paramName, object? actualValue, string? message) : base(paramName, actualValue, message)
    {
    }
}

public class ArgEx : ArgumentException, IEnhancedArgumentException
{
    protected static string GetMessage(Argument argument, string? info)
    {
        var builder = new StringBuilder();
        builder.RenderArgument(argument)
            .Append(" was invalid");
        if (!string.IsNullOrEmpty(info))
        {
            builder.Append(": ").Append(info);
        }
        return builder.ToString();
    }
    
    public Argument Argument { get; }

    public override string Message => this.RefMessageField()!;

    public ArgEx(Argument argument, string? info = null, Exception? innerException = null) 
        : base(GetMessage(argument, info), argument.Name, innerException)
    {
        this.Argument = argument;
    }
}

