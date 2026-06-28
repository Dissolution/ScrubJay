//using System.Collections.Concurrent;
//using System.Reflection;
//using System.Reflection.Emit;
//using ScrubJay.Reflection.Lightweight;
//
//namespace ScrubJay.Errors.Utilities;
//
//internal static class ExceptionFields
//{
//    private delegate ref TField? RefExceptionFieldDelegate<in TException, TField>(TException exception)
//        where TException : Exception;
//
//    private static readonly ConcurrentDictionary<(Type ExceptionType, string FieldName), Delegate> _cache = [];
//
//    private static RefExceptionFieldDelegate<TException, TField> CreateFieldRefDelegate<TException, TField>((Type ExceptionType, string FieldName) key)
//        where TException : Exception
//    {
//        var exceptionField = key.ExceptionType
//            .GetField(key.FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
//        if (exceptionField is null)
//            throw new MissingFieldException(Type.Render(key.ExceptionType), key.FieldName);
//
//        return DynamicMethod.GenerateDelegate<RefExceptionFieldDelegate<TException, TField>>(
//            $"{Type.Render(key.ExceptionType)}.{key.FieldName}",
//            gen => gen
//                .Ldarg(0)
//                .Ldflda(exceptionField)
//                .Ret());
//    }
//
//    public static ref TField? RefExceptionField<TException, TField>(TException exception, string fieldName)
//        where TException : Exception
//    {
//        var del = _cache.GetOrAdd((typeof(TException), fieldName), CreateFieldRefDelegate<TException, TField>) as RefExceptionFieldDelegate<TException, TField>;
//        if (del is null)
//            throw new MissingFieldException(Type.Render<TException>(), fieldName);
//        return ref del(exception);
//    }
//
//

//
//#if NET8_0_OR_GREATER
//    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_actualValue")]
//    public static extern ref object? RefActualValueField(ArgumentOutOfRangeException exception);
//#else
//    public static ref object? RefActualValueField(ArgumentOutOfRangeException exception)
//    {
//        return ref RefExceptionField<ArgumentOutOfRangeException, object?>(exception,
//#if NETFRAMEWORK
//            "m_actualValue"
//#else
//            "_actualValue"
//#endif
//        );
//    }
//#endif
//
//
//
//

//
//#if NET8_0_OR_GREATER
//    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_data")]
//    public static extern ref IDictionary? RefDataField(Exception exception);
//#else
//    public static ref IDictionary? RefDataField(Exception exception)
//    {
//        return ref RefExceptionField<Exception, IDictionary>(exception, "_data");
//    }
//#endif
//
//
//
//}