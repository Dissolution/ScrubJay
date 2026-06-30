namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class UniversalExtensions
{
    extension<E>(E)
        where E : IEquatable<E>
    {
        public static bool Equals(E? left, E? right)
        {
            if (left is not null)
            {
                return left.Equals(right!);
            }
            else if (right is not null)
            {
                return right.Equals(left!);
            }
            else
            {
                return true; // both are null
            }
        }
    }


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