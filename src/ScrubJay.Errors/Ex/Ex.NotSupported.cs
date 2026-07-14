using ScrubJay.Reflection.Lightweight;

namespace ScrubJay.Errors;

partial class Ex
{
    public static NotSupportedException NotSupported(
        Type instanceType,
        string? info = null,
        Exception? innerException = null,
        [CallerMemberName] string? memberName = null)
    {
        DefaultInterpolatedStringHandler builder = $"Calling {TypeName.For(instanceType)}.{memberName} is not supported";
        if (!string.IsNullOrEmpty(info))
        {
            builder.AppendLiteral(" - ");
            builder.AppendLiteral(info!);
        }
        string message = builder.ToStringAndClear();
        return new NotSupportedException(message, innerException);
    }
    
    public static NotSupportedException NotSupported<I>(
        ref readonly I? instance,
        string? info = null,
        Exception? innerException = null,
        [CallerMemberName] string? memberName = null)
#if NET9_0_OR_GREATER
    where I : allows ref struct
#endif
    {
        DefaultInterpolatedStringHandler builder = $"Calling {TypeName.For(instance)}.{memberName} is not supported";
        if (!string.IsNullOrEmpty(info))
        {
            builder.AppendLiteral(" - ");
            builder.AppendLiteral(info!);
        }
        string message = builder.ToStringAndClear();
        return new NotSupportedException(message, innerException);
    }

    public static NotSupportedException NotSupported(
        string? info = null,
        Exception? innerException = null,
        [CallerMemberName] string? memberName = null)
    {
        DefaultInterpolatedStringHandler builder = $"Calling {memberName} is not supported";
        if (!string.IsNullOrEmpty(info))
        {
            builder.AppendLiteral(" - ");
            builder.AppendLiteral(info!);
        }
        string message = builder.ToStringAndClear();
        return new NotSupportedException(message, innerException);
    }
}