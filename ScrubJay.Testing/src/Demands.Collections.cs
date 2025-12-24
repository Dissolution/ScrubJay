namespace ScrubJay.Testing;

partial class Demands
{
#if NETSTANDARD
    extension<A, T>(A actual)
        where A : struct, IActual<T[]?>
        where T : IEquatable<T>
    {
        public A IsEqualTo(scoped ReadOnlySpan<T> other)
        {
            if (actual.Value.SequenceEqual(other))
                return actual;
            throw new ActualException(actual.Realize(), $"was not equal to {other}");
        }
    }
#endif

    extension<A, T>(A actual)
        where A : struct, IActual<T[]?>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        public A IsEmpty()
        {
            if (actual.Value is null)
                throw new ActualException(actual.Realize(), $"was null");

            if (actual.Value!.Length == 0)
                return actual;

            throw new ActualException(actual.Realize(), $"was not empty");
        }

#if !NETSTANDARD
        public A IsEqualTo(scoped ReadOnlySpan<T> other)
        {
            if (actual.Value.SequenceEqual(other))
                return actual;
            throw new ActualException(actual.Realize(), $"was not equal to {other}");
        }
#endif

        public A AllEqualTo(T? other)
        {
            if (actual.Value is null)
                return actual;

            foreach (var value in actual.Value!)
            {
                if (!EqualityComparer<T>.Default.Equals(value, other!))
                {
                    throw new ActualException(actual.Realize(), $"items were not all equal to {other}");
                }
            }

            return actual;
        }
    }


#if NET9_0_OR_GREATER
    extension<T>(Actual<ReadOnlySpan<T>> actual)
    {
        public Actual<ReadOnlySpan<T>> IsEmpty()
        {
            if (actual.Value.IsEmpty)
                return actual;
            throw new ActualException(actual.Realize(), $"was not empty");
        }

        public Actual<ReadOnlySpan<T>> IsEqualTo(scoped ReadOnlySpan<T> other)
        {
            if (actual.Value.SequenceEqual(other))
                return actual;
            throw new ActualException(actual.Realize(), $"was not equal to {other}");
        }

        public Actual<ReadOnlySpan<T>> AllEqualTo(T? other)
        {
            foreach (var value in actual.Value)
            {
                if (!EqualityComparer<T>.Default.Equals(value, other))
                {
                    throw new ActualException(actual.Realize(), $"items were not all equal to {other}");
                }
            }

            return actual;
        }
    }
#endif
}