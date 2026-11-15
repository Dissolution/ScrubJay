namespace ScrubJay.Functional.Extensions;

[PublicAPI]
public static class EnumerableExtensions
{
    extension<E, T>(ref E enumerator)
        where E : struct, IEnumerator<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        public Option<T> Next()
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            return default;
        }
    }

    extension<E, T>(E enumerator)
        where E : class, IEnumerator<T>
    {
        public Option<T> Next()
        {
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            return default;
        }
    }
}