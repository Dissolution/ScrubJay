namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class PredicateExtensions
{
    extension<T>(Func<T, bool>)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        public static Func<T, bool> operator &(Func<T, bool> left, Func<T, bool> right)
        {
            return value => left(value) && right(value);
        }
    }
}