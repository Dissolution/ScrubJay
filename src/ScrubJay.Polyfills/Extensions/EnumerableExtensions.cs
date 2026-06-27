namespace ScrubJay.Polyfills;

[PublicAPI]
public static class EnumerableExtensions
{
    public static T One<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is null)
            throw new ArgumentNullException(nameof(enumerable));
        using var e = enumerable.GetEnumerator();
        if (!e.MoveNext())
            throw new InvalidOperationException("No items");
        T value = e.Current;
        if (!e.MoveNext())
            throw new InvalidOperationException("More than one item");
        return value;
    }

    public static T OneOr<T>(this IEnumerable<T>? enumerable, T fallback)
    {
        if (enumerable is null)
            return fallback;
        using var e = enumerable.GetEnumerator();
        if (!e.MoveNext())
            return fallback;
        T value = e.Current;
        if (!e.MoveNext())
            return fallback;
        return value;
    }
    
    public static T? OneOrDefault<T>(this IEnumerable<T>? enumerable)
    {
        if (enumerable is null)
            return default;
        using var e = enumerable.GetEnumerator();
        if (!e.MoveNext())
            return default;
        T value = e.Current;
        if (!e.MoveNext())
            return default;
        return value;
    }
}