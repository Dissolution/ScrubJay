namespace ScrubJay.Errors;

public static partial class Ex
{
    public static NotSupportedException NotSupported(
        Type instanceType,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerMemberName] string? memberName = null)
    {
        var message = CreateMessage($"Calling {instanceType.Alias}.{memberName} is not supported.", additionalInfo);
        return new NotSupportedException(message, innerException);
    }

    public static NotSupportedException NotSupported<I>(
        I? instance,
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerMemberName] string? memberName = null)
#if NET9_0_OR_GREATER
        where I : allows ref struct
#endif
    {
        var message = CreateMessage($"Calling {Anything.GetType(instance).Alias}.{memberName} is not supported.", additionalInfo);
        return new NotSupportedException(message, innerException);
    }

    public static NotSupportedException NotSupported(
        string? additionalInfo = null,
        Exception? innerException = null,
        [CallerMemberName] string? memberName = null)
    {
        var message = CreateMessage($"Calling {memberName} is not supported.", additionalInfo);
        return new NotSupportedException(message, innerException);
    }
}