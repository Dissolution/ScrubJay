namespace ScrubJay.Comparison;

public static partial class Relate
{
    public static int Compare(scoped text left, scoped text right)
    {
        return left.CompareTo(right, StringComparison.Ordinal);
    }

    public static int Compare(scoped text left, scoped text right, StringComparison comparison)
    {
        return left.CompareTo(right, comparison);
    }


    public static int Compare<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
    {
        return left.SequenceCompareTo(right);
    }

    public static int Compare<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, IComparer<T>? itemComparer)
    {
        return left.SequenceCompareTo(right, itemComparer);
    }


    [OverloadResolutionPriority(100)]
    public static int Compare<T>(T? left, T? right, Constrain.HasIComparable<T, T> _ = default)
        where T : IComparable<T>
    {
        if (left is not null)
            return left.CompareTo(right);
        if (right is not null)
            return -right.CompareTo(left);
        return 0; // null == null
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(95)]
    public static int Compare<T>(T? left, T? right, Constrain.AllowsRefStruct.HasIComparable<T, T> _ = default)
        where T : IComparable<T>, allows ref struct
    {
        if (left is not null)
            return left.CompareTo(right);
        if (right is not null)
            return -right.CompareTo(left);
        return 0; // null == null
    }
#endif

    [OverloadResolutionPriority(90)]
    public static int Compare<L, R>(L? left, R? right)
        where L : IComparable<R>
    {
        if (left is not null)
            return left.CompareTo(right);
        return right is null ? 0 : -1; // null < non-null
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(85)]
    public static int Compare<L, R>(L? left, R? right, Constrain.AllowsRefStruct.HasIComparable<L, R> _ = default)
        where L : IComparable<R>, allows ref struct
        where R : allows ref struct
    {
        if (left is not null)
            return left.CompareTo(right);
        return right is null ? 0 : -1; // null < non-null
    }
#endif

    [OverloadResolutionPriority(60)]
    public static int Compare<T>(T? left, T? right, Constrain.Unbounded<T> _ = default)
    {
        return Comparer<T>.Default.Compare(left, right);
    }

    [OverloadResolutionPriority(55)]
    public static int Compare<T>(T? left, T? right, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        //return Anything.Equals(in left, in right);
        throw new NotImplementedException();
    }
}