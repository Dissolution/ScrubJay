namespace ScrubJay.Errors.Utilities;

internal static class ExceptionAccess
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    public static extern ref string? RefMessage(Exception exception);
#else
//    public static readonly ReferenceFieldRef<Exception, string?> RefMessageField
//        = GetReferenceFieldRef<Exception, string?>("_message");
#endif

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_innerException")]
    public static extern ref Exception? RefInnerException(Exception exception);
#else
//    public static readonly ReferenceFieldRef<Exception, Exception?> RefInnerExceptionField
//        = GetReferenceFieldRef<Exception, Exception?>("_innerException");
#endif


#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_paramName")]
    public static extern ref string? RefParamName(ArgumentException exception);
#else
//    public static readonly ReferenceFieldRef<Exception, Exception?> RefInnerExceptionField
//        = GetReferenceFieldRef<Exception, Exception?>("_innerException");
#endif

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_actualValue")]
    public static extern ref object? RefActualValue(ArgumentOutOfRangeException exception);
#else
//    public static readonly ReferenceFieldRef<Exception, Exception?> RefInnerExceptionField
//        = GetReferenceFieldRef<Exception, Exception?>("_innerException");
#endif
}