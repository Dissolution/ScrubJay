namespace ScrubJay.Polyfills;

[PublicAPI]
public static class ReadOnlySpanExtensions
{
    extension<T>(ReadOnlySpan<T> span)
    {
        public bool Any(Func<T, bool> predicate)
        {
            foreach (T item in span)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }
        
        public bool All(Func<T, bool> predicate)
        {
            foreach (T item in span)
            {
                if (!predicate(item))
                    return false;
            }
            return true;
        }
    }
}