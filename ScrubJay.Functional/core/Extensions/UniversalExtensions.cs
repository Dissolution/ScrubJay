namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class UniversalExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> IsNotNull<T>(this Nullable<T> nullable)
        where T : struct
    {
        if (nullable.HasValue)
            return Some(nullable.GetValueOrDefault());
        return None;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> IsNotNull<T>(
        [AllowNull, NotNullWhen(true)]
        this T? maybeNull)
        where T : class
    {
        if (maybeNull is not null)
            return Some(maybeNull!);
        return None;
    }
}