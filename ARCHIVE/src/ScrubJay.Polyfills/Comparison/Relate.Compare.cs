namespace ScrubJay.Polyfills.Comparison;

public static partial class Relate
{
#region Compare(T,T)
    [OverloadResolutionPriority(100)]
    public static int Compare<T>(T? left, T? right)
        where T : IComparable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
        {
            return left.CompareTo(right!);
        }
        else if (right is not null)
        {
            return -(right.CompareTo(left!));
        }
        else
        {
            return 0;
        }
    }

    [OverloadResolutionPriority(75)]
    public static int Compare<T>(
        T? left,
        T? right,
        IComparer<T>? comparer)
        where T : IComparable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (comparer is not null)
        {
            return comparer.Compare(left!, right!);
        }
        else if (left is not null)
        {
            return left.CompareTo(right!);
        }
        else if (right is not null)
        {
            return -(right.CompareTo(left!));
        }
        else
        {
            return 0;
        }
    }
#endregion

#region Compare(ROSpan, ROSpan)
    [OverloadResolutionPriority(100)]
    public static int Compare<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right)
        where T : IComparable<T>
    {
        return MemoryExtensions.SequenceCompareTo<T>(left, right);
    }

    [OverloadResolutionPriority(75)]
    public static int Compare<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IComparer<T>? itemComparer)
        where T : IComparable<T>
    {
#if NET10_0_OR_GREATER
        return MemoryExtensions.SequenceCompareTo<T>(left, right, itemComparer);
#else
        if (itemComparer is not null)
        {
            int minLength = Math.Min(left.Length, right.Length);
            for (int i = 0; i < minLength; i++)
            {
                int c = itemComparer.Compare(left[i], right[i]);
                if (c != 0)
                {
                    return c;
                }
            }
        }
        else
        {
            int minLength = Math.Min(left.Length, right.Length);
            for (int i = 0; i < minLength; i++)
            {
                int c = Compare<T>(left[i], right[i]);
                if (c != 0)
                {
                    return c;
                }
            }
        }
        
        return left.Length.CompareTo(right.Length);
#endif
}
#endregion
}