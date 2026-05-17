namespace ScrubJay.Errors;

partial class Ex
{
    public static ArgRangeException ArgRange<T>(
        in T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        var arg = Argument.Capture<T>(in argument, argumentName);
        return new ArgRangeException(arg, argument, message);
    }

#if NET9_0_OR_GREATER
    public static ArgRangeException ArgRange<T>(
        in T? argument,
        string? message = null,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null,
        // ReSharper disable once MethodOverloadWithOptionalParameter
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        var arg = Argument.Capture<T>(in argument, argumentName, _);
        Any.TryBox(argument, out var box);
        return new ArgRangeException(arg, box, message);
    }
#endif
    
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