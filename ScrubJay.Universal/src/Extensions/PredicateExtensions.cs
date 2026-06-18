namespace ScrubJay.Universal.Extensions;

[PublicAPI]
public static class Predicate
{
    public static Predicate<T> True<T>() 
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Predicate<T>.True;
    
    public static Predicate<T> False<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => Predicate<T>.False;

    public static Predicate<T> All<T>(params ReadOnlySpan<Predicate<T>?> predicates)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Predicate<T>? predicate = null;

        for (var i = 0; i < predicates.Length; i++)
        {
            if (predicates[i].IsNotNull(out var pred))
            {
                predicate = And(predicate, pred);
            }
        }

        return predicate ?? Predicate<T>.True;
    }

    public static Predicate<T> Any<T>(params ReadOnlySpan<Predicate<T>?> predicates) 
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        Predicate<T>? predicate = null;

        for (var i = 0; i < predicates.Length; i++)
        {
            if (predicates[i].IsNotNull(out var pred))
            {
                predicate = Or(predicate, pred);
            }
        }

        return predicate ?? Predicate<T>.True;
    }
    

    [return: NotNullIfNotNull(nameof(left)), NotNullIfNotNull(nameof(right))]
    public static Predicate<T>? And<T>(Predicate<T>? left, Predicate<T>? right)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (left is null)
            return right;
        if (right is null)
            return left;
        return value => left(value) && right(value);
    }

    [return: NotNullIfNotNull(nameof(left)), NotNullIfNotNull(nameof(right))]
    public static Predicate<T>? Or<T>(Predicate<T>? left, Predicate<T>? right)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (left is null)
            return right;
        if (right is null)
            return left;
        return value => left(value) || right(value);
    }

    [return: NotNullIfNotNull(nameof(left)), NotNullIfNotNull(nameof(right))]
    public static Predicate<T>? Xor<T>(Predicate<T>? left, Predicate<T>? right)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (left is null)
            return right;
        if (right is null)
            return left;
        return value => left(value) ^ right(value);
    }
}

[PublicAPI]
public static class PredicateExtensions
{
    extension<T>(Predicate<T>)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        public static Predicate<T> True => static _ => true;

        public static Predicate<T> False => static _ => false;

        public static Predicate<T> All(params ReadOnlySpan<Predicate<T>?> predicates) => Predicate.All<T>(predicates);

        public static Predicate<T> Any(params ReadOnlySpan<Predicate<T>?> predicates) => Predicate.Any<T>(predicates);

        /* https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/expressions#12173-user-defined-conditional-logical-operators
         * `x && y` -> T.false(x) ? x : T.&(x, y)
         * `x || y` -> T.true(x) ? x : T.|(x, y)
         */

//        public static bool operator true(Predicate<T>? predicate) => predicate is not null;
//        public static bool operator false(Predicate<T>? predicate) => predicate is null;
//        
        public static Predicate<T>? operator &(Predicate<T>? left, Predicate<T>? right)
        {
            if (left is null)
                return right;
            if (right is null)
                return left;
            return value => left(value) && right(value);
        }

        public static Predicate<T>? operator |(Predicate<T>? left, Predicate<T>? right)
        {
            if (left is null)
                return right;
            if (right is null)
                return left;
            return value => left(value) || right(value);
        }
    }


    extension<T>(Predicate<T>? predicate)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        public Predicate<T>? And(Predicate<T>? other)
        {
            if (other is null)
                return predicate;
            if (predicate is null)
                return other;
            return value => predicate(value) && other(value);
        }

        public Predicate<T>? Or(Predicate<T>? other)
        {
            if (other is null)
                return predicate;
            if (predicate is null)
                return other;
            return value => predicate(value) || other(value);
        }
    }
}