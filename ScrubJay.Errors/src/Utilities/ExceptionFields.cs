using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Errors.Utilities;

internal static class ExceptionFields
{
    private delegate ref TField? RefExceptionFieldDelegate<in TException, TField>(TException exception)
        where TException : Exception;

    private static readonly ConcurrentDictionary<(Type ExceptionType, string FieldName), Delegate> _cache = [];

    private static RefExceptionFieldDelegate<TException, TField> CreateFieldRefDelegate<TException, TField>((Type ExceptionType, string FieldName) key)
        where TException : Exception
    {
        var exceptionField = key.ExceptionType
            .GetField(key.FieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (exceptionField is null)
            throw new MissingFieldException(Type.Render(key.ExceptionType), key.FieldName);

        var method = Any.CreateDynamicMethod<RefExceptionFieldDelegate<TException, TField>>($"{Type.Render(key.ExceptionType)}.{key.FieldName}");
        var gen = method.GetILGenerator();
        gen.Emit(OpCodes.Ldarg_0);
        gen.Emit(OpCodes.Ldflda, exceptionField);
        gen.Emit(OpCodes.Ret);
        return method.CreateDelegate<RefExceptionFieldDelegate<TException, TField>>();
    }

    public static ref TField? RefExceptionField<TException, TField>(TException exception, string fieldName)
        where TException : Exception
    {
        var del = _cache.GetOrAdd((typeof(TException), fieldName), CreateFieldRefDelegate<TException, TField>) as RefExceptionFieldDelegate<TException, TField>;
        if (del is null)
            throw new MissingFieldException(Type.Render<TException>(), fieldName);
        return ref del(exception);
    }


#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_paramName")]
    public static extern ref string? RefParamNameField(ArgumentException exception);
#else
    public static ref string? RefParamNameField(ArgumentException exception)
    {
        return ref RefExceptionField<ArgumentException, string?>(exception,
#if NETFRAMEWORK
            "m_paramName"
#else
            "_paramName"
#endif
        );
    }
#endif

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_actualValue")]
    public static extern ref object? RefActualValueField(ArgumentOutOfRangeException exception);
#else
    public static ref object? RefActualValueField(ArgumentOutOfRangeException exception)
    {
        return ref RefExceptionField<ArgumentOutOfRangeException, object?>(exception,
#if NETFRAMEWORK
            "m_actualValue"
#else
            "_actualValue"
#endif
        );
    }
#endif




#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    public static extern ref string? RefMessageField(Exception exception);
#else
    public static ref string? RefMessageField(Exception exception)
    {
        return ref RefExceptionField<Exception, string?>(exception, "_message");
    }
#endif

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_innerException")]
    public static extern ref Exception? RefInnerExceptionField(Exception exception);
#else
    public static ref Exception? RefInnerExceptionField(Exception exception)
    {
        return ref RefExceptionField<Exception, Exception?>(exception, "_innerException");
    }
#endif

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_data")]
    public static extern ref IDictionary? RefDataField(Exception exception);
#else
    public static ref IDictionary? RefDataField(Exception exception)
    {
        return ref RefExceptionField<Exception, IDictionary>(exception, "_data");
    }
#endif



}