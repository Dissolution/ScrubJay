namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Throw
{
    public static void IfNull<T>([AllowNull, NotNull] T value,
        [CallerArgumentExpression(nameof(value))]
        string? valueName = null)
    {
        if (value is not null)
            return;
        throw Ex.ArgNull(value, valueName: valueName);
    }
}