#nullable enable

namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Validate
{
    public static Result<T> IsNotNull<T>([AllowNull] T actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
    {
        if (actual is not null)
            return actual;
        return Ex.ArgNull(actualName);
    }
    
    public static Result<Nullable<T>> IsNotNull<T>([AllowNull] Nullable<T> actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : struct
    {
        if (actual.HasValue)
            return actual;
        return Ex.ArgNull(actualName);
    }
    
    public static Result<T?> IsNull<T>(T? actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
    {
        if (actual is null)
            return actual;
        return Ex.Arg(actual, "was not null", actualName);
    }
    
    public static Result<Nullable<T>> IsNull<T>(Nullable<T> actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : struct
    {
        if (!actual.HasValue)
            return actual;
        return Ex.Arg(actual, "was not null", actualName);
    }
}
