//using ScrubJay.Universal.Extensions;
//
//namespace ScrubJay.Errors.Validation;
//
//public static class ThrowExtensions
//{
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
//}