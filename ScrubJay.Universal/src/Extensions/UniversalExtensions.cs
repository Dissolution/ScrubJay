namespace ScrubJay.Universal.Extensions;

/// <summary>
///
/// </summary>
[PublicAPI]
public static class UniversalExtensions
{


    extension<T>(Nullable<T> nullable)
        where T : struct
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(out T value)
        {
            value = nullable.GetValueOrDefault();
            return nullable.HasValue;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(this Nullable<T> nullable, out T nonNullValue)
        where T : struct
    {
        nonNullValue = nullable.GetValueOrDefault();
        return nullable.HasValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(
        [AllowNull, NotNullWhen(true)]
        this T? maybeNull,
        [NotNullWhen(true)]
        out T? nonNullValue)
        where T : class
    {
        nonNullValue = maybeNull;
        return maybeNull is not null;
    }
}