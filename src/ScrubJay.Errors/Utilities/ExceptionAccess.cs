#if !NET8_0_OR_GREATER
using ScrubJay.Reflection.Lightweight;
#endif

namespace ScrubJay.Errors.Utilities;

internal static class ExceptionAccess
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    public static extern ref string? RefMessageField(Exception exception);
#else
    public static UnsafeAccessor.ReferenceFieldRef<Exception, string?> RefMessageField
        = UnsafeAccessor.GetReferenceFieldRef<Exception, string?>("_message");
#endif

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_innerException")]
    public static extern ref Exception? RefInnerExceptionField(Exception exception);
#else
    public static UnsafeAccessor.ReferenceFieldRef<Exception, string?> RefInnerExceptionField
        = UnsafeAccessor.GetReferenceFieldRef<Exception, string?>("_innerException");
#endif




//
//
//
//
//
//
//#if NET8_0_OR_GREATER
//    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_paramName")]
//    public static extern ref string? RefParamNameField(ArgumentException exception);
//#else
//public static UnsafeAccessor.ReferenceFieldRef<ArgumentException, string?> RefParamNameField
//    = UnsafeAccessor.GetReferenceFieldRef<ArgumentException, string?>(
//#if NETFRAMEWORK
//            "m_paramName"
//#else
//        "_paramName"
//#endif
//    );
//#endif
}