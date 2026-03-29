using System.Linq.Expressions;
using ScrubJay.Enhancements.Expressions;
using ScrubJay.Enhancements.Text.Building;
using ScrubJay.Enhancements.Rendering;

namespace ScrubJay.Enhancements.Exceptions;

partial class Ex
{
    public static EnhancedArgumentOutOfRangeException ArgRange<TArgument, TRange>(
        ref readonly TArgument? argument,
        in TRange? range,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where TArgument : allows ref struct
        where TRange : allows ref struct
#endif
    {
        return new EnhancedArgumentOutOfRangeException(
            Argument.Capture(in argument, argumentName),
            Any.ToString<TRange>(in range),
            info);
    }
    
    public static EnhancedArgumentOutOfRangeException ArgRange<T>(
        ref readonly T? argument,
        string? range,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new EnhancedArgumentOutOfRangeException(
            Argument.Capture(in argument, argumentName),
            range,
            info);
    }
    
    public static EnhancedArgumentOutOfRangeException ArgRange<T>(
        ref readonly T? argument,
        Expression<Func<T?, bool>> range,
        string? info = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return new EnhancedArgumentOutOfRangeException(
            Argument.Capture(in argument, argumentName),
            range.Render(),
            info);
    }
}