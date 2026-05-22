namespace ScrubJay.Functional.Extensions;

/// <summary>
/// Extensions on <see cref="IEnumerable{T}"/> and similar types
/// </summary>
[PublicAPI]
public static class EnumerableExtensions
{
    private sealed class TryEnumerable<E, T> : IEnumerable<T>, IEnumerable
        where E : IEnumerable<T>
    {
        private readonly E? _enumerable;

        public TryEnumerable(E? enumerable)
        {
            _enumerable = enumerable;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            if (_enumerable is not null)
            {
                try
                {
                    var enumerator = _enumerable.GetEnumerator();
                    return new TryEnumerator<IEnumerator<T>, T>(enumerator);
                }
                catch
                {
                    // ignore all
                }
            }
            return Enumerable.Empty<T>().GetEnumerator();
        }
    }

    private sealed class TryEnumerator<E, T> : IEnumerator<T>, IEnumerator, IDisposable
        where E : IEnumerator<T>
    {
        private E? _enumerator;

        object? IEnumerator.Current => Current;

        public T Current
        {
            get
            {
                if (_enumerator is not null)
                {
                    try
                    {
                        return _enumerator.Current;
                    }
                    catch
                    {
                        // ignore all
                    }
                }
                return default!;
            }
        }

        public TryEnumerator(E? enumerator)
        {
            _enumerator = enumerator;
        }

        public bool TryMoveNext([MaybeNullWhen(false)] out T? value)
        {
            if (_enumerator is not null)
            {
                try
                {
                    bool moved = _enumerator.MoveNext();
                    if (moved)
                    {
                        value = _enumerator.Current;
                        return true;
                    }
                }
                catch
                {
                    // ignore all exceptions                 
                }
            }

            value = default;
            return false;
        }

        public bool MoveNext()
        {
            if (_enumerator is not null)
            {
                try
                {
                    bool moved = _enumerator.MoveNext();
                    if (moved)
                    {
                        _ = _enumerator.Current; // have to be able to get current to consider this moved
                        return true;
                    }
                }
                catch
                {
                    // ignore all exceptions                 
                }
            }

            return false;
        }

        public void Reset()
        {
            if (_enumerator is not null)
            {
                try
                {
                    _enumerator.Reset();
                }
                catch
                {
                    // ignore all
                }
            }
        }

        public void Dispose()
        {
            if (_enumerator is not null)
            {
                try
                {
                    _enumerator.Dispose();
                    _enumerator = default;
                }
                catch
                {
                    // ignore all
                }
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
                if (selectWhere(value)
                    .IsSome(out var newValue))
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
                if (selectWhere(value)
                    .IsOk(out var newValue))
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
                if (selectWhere(value)
                    .IsOk(out var newValue))
                {
                    yield return newValue;
                }
            }
        }
#endregion

#region TryLINQ
        public IEnumerable<T> TryEnumerate()
        {
            return new TryEnumerable<IEnumerable<T>, T>(enumerable);
        }

        public IEnumerable<N> TrySelect<N>(Func<T, N>? selector)
        {
            if (enumerable is null || selector is null)
                yield break;

            foreach (T value in enumerable)
            {
                N newValue;

                try
                {
                    newValue = selector(value);
                }
                catch
                {
                    continue;
                }

                yield return newValue;
            }
        }

        public IEnumerable<T> TryWhere(Func<T, bool>? predicate)
        {
            if (enumerable is null || predicate is null)
                yield break;

            foreach (T value in enumerable)
            {
                try
                {
                    if (!predicate(value))
                        continue;
                }
                catch
                {
                    continue;
                }

                yield return value;
            }
        }

        public IEnumerable<N> TrySelectMany<N>(Func<T, IEnumerable<N>?>? selector)
        {
            if (enumerable is null || selector is null)
                yield break;

            foreach (T value in enumerable)
            {
                IEnumerable<N>? many = null;

                try
                {
                    many = selector(value);
                }
                catch
                {
                    continue;
                }

                foreach (var n in TryEnumerate<N>(many))
                {
                    yield return n;
                }
            }
        }
#endregion
    }
}