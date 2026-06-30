#if !NET8_0_OR_GREATER
using static ScrubJay.Reflection.Lightweight.UnsafeAccessor;
#endif

namespace ScrubJay.Errors.Utilities;

internal static class ExceptionAccess
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_message")]
    public static extern ref string? RefMessageField(Exception exception);
#else
    public static readonly ReferenceFieldRef<Exception, string?> RefMessageField
        = GetReferenceFieldRef<Exception, string?>("_message");
#endif

#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_innerException")]
    public static extern ref Exception? RefInnerExceptionField(Exception exception);
#else
    public static readonly ReferenceFieldRef<Exception, Exception?> RefInnerExceptionField
        = GetReferenceFieldRef<Exception, Exception?>("_innerException");
#endif
}