using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors.Validation;

[PublicAPI]
public static class ThrowExtensions
{
    [return: NotNull]
    public static T ThrowIfNull<T>(
        [AllowNull, NotNull] this T? value,
        string? info = null,
        [CallerArgumentExpression(nameof(value))]
        string? valueName = null)
    {
        if (value is null)
        {
            ArgNullException.Throw(in value, info, null, valueName);
        }
        return value;
    }
    
    
//    [DoesNotReturn]
//    private static void ThrowObjectNotTypeException<T>(object? obj, string? info, string? objName)
//    {
//        DefaultInterpolatedStringHandler message = new();
//        message.Write(Any.GetType(obj));
//        message.Write(" '");
//        message.Write(objName);
//        message.Write("' is not a valid ");
//        message.Write(typeof(T));
//        if (!info.IsNullOrEmpty())
//        {
//            message.Write(": ");
//            message.Write(info);
//        }
//        throw new ArgumentException(message.ToStringAndClear(), objName);
//    }
//    
//    public static T ThrowIfNot<T>(
//        this object? obj,
//        string? info = null,
//        [CallerArgumentExpression(nameof(obj))]
//        string? objName = null)
//    {
//        if (obj is T value)
//        {
//            return value;
//        }
//        
//        ThrowObjectNotTypeException<T>(obj, info, objName);
//        throw new UnreachableException();
//    }
}