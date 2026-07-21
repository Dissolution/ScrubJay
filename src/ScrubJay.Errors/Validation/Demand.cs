using ScrubJay.Errors.Exceptions;

namespace ScrubJay.Errors.Validation;

[PublicAPI]
public static partial class Demand
{
    public static ref readonly T InRange<T>(in T value, T incMin, T exclMax,
        [CallerArgumentExpression(nameof(value))]
        string? argumentName = null)
        where T : IComparable<T>
    {
        if ((Comparer<T>.Default.Compare(value!, incMin!) < 0)
            || (Comparer<T>.Default.Compare(value!, exclMax!) >= 0))
        {
            ArgRangeException.Throw(value,
                info: $"was not in [{incMin}..{exclMax})",
                argumentName: argumentName);
        }
        return ref value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNull]
    public static T NotNull<T>([AllowNull, NotNull] T? argument,
        [CallerArgumentExpression(nameof(argument))]
        string? argumentName = null)
    {
        if (argument is null)
            ArgNullException.Throw<T>(in argument, argumentName: argumentName);
        return argument;
    }
}