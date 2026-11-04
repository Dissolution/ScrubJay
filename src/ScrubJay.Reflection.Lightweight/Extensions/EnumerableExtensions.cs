namespace ScrubJay.Reflection.Lightweight.Extensions;

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> enumerable)
    {
        public T One()
        {
            if (enumerable.TryGetNonEnumeratedCount(out var count))
            {
                if (count == 1)
                {
                    using var e = enumerable.GetEnumerator();
                    _ = e.MoveNext();
                    return e.Current;
                }

                throw new InvalidOperationException($"There are {count} values in the enumerable");
            }
            else
            {
                using var e = enumerable.GetEnumerator();
                if (!e.MoveNext())
                    throw new InvalidOperationException("There are zero values in the enumerable");
                var one = e.Current;
                if (e.MoveNext())
                    throw new InvalidOperationException("There is more than one value in the enumerable");
                return one;
            }
        }

        public T? OneOrDefault()
        {
            if (enumerable.TryGetNonEnumeratedCount(out var count))
            {
                if (count == 1)
                {
                    using var e = enumerable.GetEnumerator();
                    _ = e.MoveNext();
                    return e.Current;
                }

                return default;
            }
            else
            {
                using var e = enumerable.GetEnumerator();
                if (!e.MoveNext())
                    return default;
                var one = e.Current;
                if (e.MoveNext())
                    return default;
                return one;
            }
        }
    }
}