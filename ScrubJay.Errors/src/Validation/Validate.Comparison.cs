namespace ScrubJay.Errors.Validation;

public partial class Validate
{
    [DoesNotReturn]
    private static void ThrowNotInRange<T>(T value, LowerBound<T> lowerBound, UpperBound<T> upperBound, string? paramName)
        where T : IComparable<T>
    {
        var arg = Argument.Capture<T>(in value, paramName);
        Any.TryBox(value, out var box);
        throw new ArgRangeException(arg, box, R($"Argument {arg:@} was not in {(Bounds.RenderTo, lowerBound, upperBound)}"));
    }

    public static Result<T> InRange<T>(T value, LowerBound<T> lowerBound, UpperBound<T> upperBound,
        [CallerArgumentExpression(nameof(value))]
        string? valueName = null)
        where T : IComparable<T>
    {
        if (!lowerBound.Contains(value) || !upperBound.Contains(value))
        {
            return Ex.ArgRange(value, lowerBound, upperBound, valueName);
        }
        return value;
    }
}