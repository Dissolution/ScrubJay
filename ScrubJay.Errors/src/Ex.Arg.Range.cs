namespace ScrubJay.Errors;

public partial class Ex
{
    public static ArgRangeException ArgRange<T>(
        in T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        return new ArgRangeException(arg, Any.BoxOrBytes(in argument), message);
    }

    public static ArgRangeException ArgRange<T>(
        T argument,
        LowerBound<T> lowerBound,
        UpperBound<T> upperBound,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
        where T : IComparable<T>
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        Any.TryBox(argument, out var box);
        return new ArgRangeException(arg, box,
            R($"Argument {arg:@} was not in {(Bounds.RenderTo, lowerBound, upperBound)}"));
    }
}