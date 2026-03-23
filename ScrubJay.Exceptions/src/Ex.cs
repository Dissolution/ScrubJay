namespace ScrubJay.Exceptions;

[PublicAPI]
public static partial class Ex
{
#region NotImplementedException
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NotImplementedException NotImplemented()
    {
        return new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NotImplementedException NotImplemented(string? message)
    {
        return new NotImplementedException(message);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NotImplementedException NotImplemented(string? message, Exception? innerException)
    {
        return new NotImplementedException(message, innerException);
    }
#endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentException Argument(Argument argument)
    {
        return new EnhancedArgumentException(argument);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentException Argument(Argument argument, string? info)
    {
        return new EnhancedArgumentException(argument, info);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentException Argument(Argument argument, string? info, Exception? innerException)
    {
        return new EnhancedArgumentException(argument, info, innerException);
    }

    public static EnhancedArgumentException Argument<T>(
        ref readonly T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new(Exceptions.Argument.Create<T>(in argument, argumentName));
    }
}