using System.Collections.Immutable;
using ScrubJay.Collections.Pooling;

namespace ScrubJay.Collections;

[PublicAPI]
public static class EnumerableExtensions
{
    public static ArrayEnumerator<T> GetEnumerator<T>(this T[]? array)
    {
        return new ArrayEnumerator<T>(array);
    }

    public static IEnumerable<T> Delimited<T>(this IEnumerable<T>? enumerable, T delimiter)
    {
        if (enumerable is null)
            yield break;

        using var e = enumerable.GetEnumerator();
        if (!e.MoveNext())
            yield break;

        yield return e.Current;

        while (e.MoveNext())
        {
            yield return delimiter;
            yield return e.Current;
        }
    }

    public static string Collect(this IEnumerable<char>? characters)
    {
        if (characters is null)
            return string.Empty;

        var builder = StringBuilder.Rent();
        foreach (char ch in characters)
        {
            builder.Append(ch);
        }
        return builder.ToStringAndReturn();
    }

    public static string Collect(this IEnumerable<string?>? strings)
    {
        if (strings is null)
            return string.Empty;

        var builder = StringBuilder.Rent();
        foreach (string? str in strings)
        {
            builder.Append(str);
        }
        return builder.ToStringAndReturn();
    }

    extension(IEnumerable<Type>? types)
    {
        public TypeSet ToTypeSet()
        {
            var typeset = new TypeSet();
            typeset.AddMany(types);
            return typeset;
        }
    }

    public static View<T> ToView<T>(this T[]? array) => View.Create<T>(array);

    [OverloadResolutionPriority(100)]
    public static View<T> ToView<T>(this ref ImmutableArray<T> immutableArray)
        => View.Create<T>(ref immutableArray);

    public static View<T> ToView<T>(this List<T>? list) => View.Create<T>(list);

    public static View<T> ToView<T>(this IEnumerable<T>? enumerable) => View.Create<T>(enumerable);
}