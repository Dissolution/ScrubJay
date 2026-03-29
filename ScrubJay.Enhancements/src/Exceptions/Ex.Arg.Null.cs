namespace ScrubJay.Enhancements.Exceptions;

public static partial class Ex
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentNullException ArgNull(Argument argument)
    {
        return new EnhancedArgumentNullException(argument);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentNullException ArgNull(Argument argument, string? info)
    {
        return new EnhancedArgumentNullException(argument, info);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentNullException ArgNull(Argument argument, string? info, Exception? innerException)
    {
        return new EnhancedArgumentNullException(argument, info, innerException);
    }

    public static EnhancedArgumentNullException ArgNull<T>(
        ref readonly T? argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new(Argument.Capture<T>(in argument, argumentName), info);
    }

    public static EnhancedArgumentNullException ArgNull(
        string? argumentName,
        Type? argumentType,
        string? info = null)
    {
        return new(Argument.Create(argumentName, argumentType, null), info);
    }
}