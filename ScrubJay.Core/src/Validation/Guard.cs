namespace ScrubJay.Validation;

[PublicAPI]
public static partial class Guard
{
    public static T IsNotNull<T>([AllowNull, NotNull] T actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
    {
        if (actual is not null)
            return actual;
        throw Ex.ArgNull<T>(actualName);
    }
    
    public static Nullable<T> IsNotNull<T>([AllowNull, NotNull] Nullable<T> actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : struct
    {
        if (actual.HasValue)
            return actual;
        throw Ex.ArgNull<T?>(actualName);
    }
    
    public static T? IsNull<T>(T? actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
    {
        if (actual is null)
            return actual;
        throw Ex.Arg(actual, "was not null", actualName);
    }
    
    public static Nullable<T> IsNull<T>(Nullable<T> actual,
        [CallerArgumentExpression(nameof(actual))] string? actualName = null)
        where T : struct
    {
        if (!actual.HasValue)
            return actual;
        throw Ex.Arg(actual, "was not null", actualName);
    }
}