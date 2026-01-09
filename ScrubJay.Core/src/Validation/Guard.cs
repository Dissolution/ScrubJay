namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Guard
{
    public static T IsNotNull<T>([AllowNull, NotNull] T actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
    {
        if (actual is not null)
            return actual;
        throw Ex.ArgNull(actualName);
    }
}