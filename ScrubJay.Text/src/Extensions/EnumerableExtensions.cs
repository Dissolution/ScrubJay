namespace ScrubJay.Text.Extensions;

public static class EnumerableExtensions
{
    public static void Consume<T>(this IEnumerable<T>? enumerable, Action<T>? perItem)
    {
        if (enumerable is not null && perItem is not null)
        {
            foreach (var item in enumerable)
            {
                perItem(item);
            }
        }
    }

    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?>? enumerable)
    {
        if (enumerable is null)
            yield break;
        foreach (T? value in enumerable)
        {
            if (value is not null)
                yield return value;
        }
    }
}