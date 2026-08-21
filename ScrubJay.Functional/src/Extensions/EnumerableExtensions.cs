namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on <see cref="IEnumerable{T}"/>
/// </summary>
[PublicAPI]
public static class EnumerableExtensions
{
    extension<C>(IEnumerable<C?>? enumerable)
        where C : class
    {
        public IEnumerable<C> WhereNotNull()
        {
            if (enumerable is null)
                yield break;
            foreach (C? item in enumerable)
            {
                if (item is not null)
                    yield return item;
            }
        }
    }

    extension<S>(IEnumerable<Nullable<S>>? enumerable)
        where S : struct
    {
        public IEnumerable<S> WhereNotNull()
        {
            if (enumerable is null)
                yield break;
            foreach (S? item in enumerable)
            {
                if (item.HasValue)
                    yield return item.GetValueOrDefault();
            }
        }
    }

    extension<T>(IEnumerable<T>? enumerable)
    {
#region SelectWhere
        public IEnumerable<N> SelectWhere<N>(Func<T, Option<N>>? selectWhere)
        {
            if (enumerable is null || selectWhere is null)
                yield break;

            foreach (T value in enumerable)
            {
                if (selectWhere(value).IsSome(out var newValue))
                {
                    yield return newValue;
                }
            }
        }

        public IEnumerable<N> SelectWhere<N>(Func<T, Result<N>>? selectWhere)
        {
            if (enumerable is null || selectWhere is null)
                yield break;

            foreach (T value in enumerable)
            {
                if (selectWhere(value).IsOk(out var newValue))
                {
                    yield return newValue;
                }
            }
        }

        public IEnumerable<N> SelectWhere<N, E>(Func<T, Result<N, E>>? selectWhere)
        {
            if (enumerable is null || selectWhere is null)
                yield break;

            foreach (T value in enumerable)
            {
                if (selectWhere(value).IsOk(out var newValue))
                {
                    yield return newValue;
                }
            }
        }
#endregion
    }
}