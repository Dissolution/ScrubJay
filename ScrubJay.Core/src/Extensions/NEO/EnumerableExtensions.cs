// ReSharper disable TooWideLocalVariableScope
// ReSharper disable InlineOutVariableDeclaration
#pragma warning  disable CA2208
namespace ScrubJay.Extensions.NEO;

[PublicAPI]
public static class EnumerableExtensions
{
    [PublicAPI]
    public delegate bool SelectWherePredicate<in I, O>(I input, out O output)
#if NET9_0_OR_GREATER
        where I : allows ref struct
        where O : allows ref struct
#endif
    ;

    extension<E, T>(E? enumerable)
        where E : IEnumerable<T>
    {
        #region One()

        /* The behavior of Enumerable.SingleOrDefault() is counter-intuitive:
         * The name suggests that if there is one item in an enumerable, that item will be returned,
         * and if there are zero or more than one items, default(T) will be returned.
         * That is not the case -- default(T) is returned if there are zero items and an Exception is thrown if there is more than one.
         *
         * This is an implementation of expected behavior.
         */

  
        public Result<T> TryGetOne()
        {
            if (enumerable is null)
            {
                return new ArgumentNullException(nameof(enumerable));
            }
            else if (enumerable is IList<T> list)
            {
                if (list.Count == 1)
                {
                    return Ok(list[0]);
                }

                return new ArgumentException($"List [{list.Count}] does not have one item", nameof(enumerable));
            }
            else if (enumerable is ICollection<T> collection)
            {
                if (collection.Count == 1)
                {
                    using var e = collection.GetEnumerator();
                    e.MoveNext();
                    return Ok(e.Current);
                }

                return new ArgumentException($"Collection ({collection.Count}) does not have one item",
                    nameof(enumerable));
            }
            else
            {
                using var e = enumerable.GetEnumerator();

                if (!e.MoveNext())
                    return new ArgumentException("Enumerable has zero items");

                T value = e.Current;

                if (e.MoveNext())
                    return new ArgumentException("Enumerable has more than one item");

                return Ok(value);
            }
        }
      

        public Result<T> TryGetOne(Func<T, bool>? predicate)
        {
            if (enumerable is null)
                return new ArgumentNullException(nameof(enumerable));

            if (predicate is null)
                return new ArgumentNullException(nameof(predicate));

            if (enumerable is IList<T> list)
            {
                int count = list.Count;

                for (int i = 0; i < count; i++)
                {
                    T value = list[i];
                    if (predicate(value))
                    {
                        for (i++; i < count; i++)
                        {
                            if (predicate(list[i]))
                            {
                                return new ArgumentException("List has more than one matching item",
                                    nameof(enumerable));
                            }
                        }

                        // exactly one matching value
                        return Ok(value);
                    }
                }

                return new ArgumentException("List has no matching items", nameof(enumerable));
            }

            using IEnumerator<T> e = enumerable.GetEnumerator();

            while (e.MoveNext())
            {
                T value = e.Current;

                if (predicate(value))
                {
                    while (e.MoveNext())
                    {
                        if (predicate(e.Current))
                        {
                            return new ArgumentException("Enumerable has more than one matching item");
                        }
                    }

                    // exactly one matching value
                    return Ok(value);
                }
            }

            return new ArgumentException("Enumerable has no matching items");
        }


        public T One() 
            => enumerable.TryGetOne<E,T>().OkOrThrow();

        public T One(Func<T, bool> predicate)
            => enumerable.TryGetOne<E,T>(predicate).OkOrThrow();

        public T? OneOrDefault()
            => enumerable.TryGetOne<E,T>().OkOrDefault();

        public T OneOr(T fallback)
            => enumerable.TryGetOne<E,T>().OkOr(fallback);

        public T? OneOrDefault(Func<T, bool> predicate)
            => enumerable.TryGetOne<E,T>(predicate).OkOrDefault();

        public T OneOr(Func<T, bool> predicate, T fallback)
            => enumerable.TryGetOne<E,T>(predicate).OkOr(fallback);

#endregion
      
    }


    extension<E, T>(E? enumerable)
        where E : IEnumerable<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        public IEnumerable<O> SelectWhere<O>(SelectWherePredicate<T, O> selectWherePredicate)
        {
            if (enumerable is null)
                yield break;

            O output;

            foreach (T value in enumerable)
            {
                if (selectWherePredicate(value, out output))
                    yield return output;
            }
        }
    }

    extension<E, T>(E? enumerable)
        where E : IEnumerable<T?>
        where T : notnull
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        public IEnumerable<T> WhereNotNull()
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

    extension<T>(IEnumerable<T> enumerable)
    {
        public IEnumerable<T> OrderByIndexIn(T[] orderedItems)
        {
            return enumerable.OrderBy(item => Array.IndexOf(orderedItems, item));
        }

        public IEnumerable<T> OrderByIndexIn(T[] orderedItems, IEqualityComparer<T>? itemComparer,
            bool firstToLast = true)
        {
            return enumerable.OrderBy(item => orderedItems.TryFindIndex(item, itemComparer, firstToLast).SomeOr(-1));
        }

        /// <summary>
        /// Consume this <see cref="IEnumerable{T}"/> by performing an <see cref="Action{T}"/> on each of its values
        /// </summary>
        /// <param name="enumerable">
        /// The <see cref="IEnumerable{T}"/> to consume (after which further enumeration will fail)
        /// </param>
        /// <param name="perItem">
        /// An <see cref="Action{T}"/> to invoke upon each value in the <see cref="IEnumerable{T}"/>
        /// </param>
        public void Consume(Action<T> perItem)
        {
            foreach (T item in enumerable)
            {
                perItem(item);
            }
        }




#if NETSTANDARD2_0
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
        => new(enumerable);
#endif
    }
}