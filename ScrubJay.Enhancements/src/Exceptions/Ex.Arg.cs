namespace ScrubJay.Enhancements.Exceptions;

public static partial class Ex
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentException Arg(Argument argument)
    {
        return new EnhancedArgumentException(argument);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentException Arg(Argument argument, string? info)
    {
        return new EnhancedArgumentException(argument, info);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EnhancedArgumentException Arg(Argument argument, string? info, Exception? innerException)
    {
        return new EnhancedArgumentException(argument, info, innerException);
    }

    public static EnhancedArgumentException Arg<T>(
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

    public static EnhancedArgumentException Arg<T>(
        scoped ReadOnlySpan<T> argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new(Argument.Create(argumentName, typeof(ReadOnlySpan<T>), argument.ToString()), info);
    }
    
    public static EnhancedArgumentException Arg<T>(
        scoped Span<T> argument,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        return new(Argument.Create(argumentName, typeof(Span<T>), argument.ToString()), info);
    }
}