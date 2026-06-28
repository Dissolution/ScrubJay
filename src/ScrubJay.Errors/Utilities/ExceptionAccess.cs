using ScrubJay.Reflection.Lightweight;


namespace ScrubJay.Errors.Utilities;

internal static class ExceptionAccess
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_paramName")]
    public static extern ref string? RefParamNameField(ArgumentException exception);
#else
    public static UnsafeAccessor.ReferenceFieldRef<ArgumentException, string?> RefParamNameField
        = UnsafeAccessor.GetReferenceFieldRef<ArgumentException, string?>(
#if NETFRAMEWORK
            "m_paramName"
#else
            "_paramName"
#endif
        );
#endif
}