namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class PredicateExtensions
{
    extension<T>(Predicate<T>)
    {
        public static Func<T, bool> True => static _ => true;

        public static Func<T, bool> False => static _ => false;
    }

    extension<T>(Func<T, bool>)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        public static Func<T, bool>? operator &(Func<T, bool>? left, Func<T, bool>? right)
        {
            if (left is null)
                return right;
            if (right is null)
                return left;
            return value => left(value) && right(value);
        }

        public static Func<T, bool>? operator |(Func<T, bool>? left, Func<T, bool>? right)
        {
            if (left is null)
                return right;
            if (right is null)
                return left;
            return value => left(value) || right(value);
        }
    }


    extension<T>(Func<T, bool>? predicate)
    {
        public Func<T, bool>? And(Func<T, bool>? other)
        {
            if (other is null)
                return predicate;
            if (predicate is null)
                return other;
            return value => predicate(value) && other(value);
        }

        public Func<T, bool>? Or(Func<T, bool>? other)
        {
            if (other is null)
                return predicate;
            if (predicate is null)
                return other;
            return value => predicate(value) || other(value);
        }
    }
}