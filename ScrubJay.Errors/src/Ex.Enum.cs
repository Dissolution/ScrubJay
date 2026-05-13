using System.ComponentModel;

namespace ScrubJay.Errors;

partial class Ex
{
    public static InvalidEnumException InvalidEnum<E>(E @enum, [CallerArgumentExpression(nameof(@enum))] string? enumName = null)
        where E : struct, Enum
    {
        return new InvalidEnumException(enumName!, ((IConvertible)@enum).ToInt32(null), typeof(E));
    }
}