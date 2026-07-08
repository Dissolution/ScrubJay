using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors.Validation;

public static partial class Validate
{
    public static ref readonly T InRange<T>(in T value, T incMin, T exclMax,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : IComparable<T>
    {
        if ((Comparer<T>.Default.Compare(value!, incMin!) < 0)
            || (Comparer<T>.Default.Compare(value!, exclMax!) >= 0))
        {
            throw ArgRangeException.Create(value, 
                info: $"was not in [{incMin}..{exclMax})",
                argumentName: argumentName);
        }
        return ref value;
    }
}