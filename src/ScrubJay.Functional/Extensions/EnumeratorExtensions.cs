namespace ScrubJay.Functional;

[PublicAPI]
public static class EnumeratorExtensions
{
    extension<E, T>(E enumerator)
        where E : class, IEnumerator<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> Next()
        {
            if (enumerator.MoveNext())
                return Some(enumerator.Current);
            return default;
        }
    }
}

[PublicAPI]
public static class EnumeratorExtensions2
{
    extension<E, T>(ref E enumerator)
        where E : struct, IEnumerator<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Option<T> Next()
        {
            if (enumerator.MoveNext())
                return Some(enumerator.Current);
            return default;
        }
    }
}