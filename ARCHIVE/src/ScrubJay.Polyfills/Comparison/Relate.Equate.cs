// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Polyfills.Comparison;

public static partial class Relate
{
#region Equate<T>(T, T)
    [OverloadResolutionPriority(100)]
    public static bool Equate<T>(T? left, T? right, TypeConstraints.RefStruct.HasIEquatable<T> _ = default)
        where T : IEquatable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
        {
            return left.Equals(right!);
        }
        else if (right is not null)
        {
            return right.Equals(left!);
        }
        else
        {
            return true;
        }
    }

    [OverloadResolutionPriority(75)]
    public static bool Equate<T>(T? left, T? right,
        IEqualityComparer<T>? comparer,
        TypeConstraints.RefStruct.HasIEquatable<T> _ = default)
        where T : IEquatable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (comparer is not null)
        {
            return comparer.Equals(left!, right!);
        }
        else if (left is not null)
        {
            return left.Equals(right!);
        }
        else if (right is not null)
        {
            return right.Equals(left!);
        }
        else
        {
            return true;
        }
    }

    [OverloadResolutionPriority(50)]
    public static bool Equate<T>(T? left, T? right, IEqualityComparer<T>? comparer)
    {
        if (comparer is not null)
        {
            return comparer.Equals(left!, right!);
        }
        return EqualityComparer<T>.Default.Equals(left!, right!);
    }

    [OverloadResolutionPriority(25)]
    public static bool Equate<T>(T? left, T? right, TypeConstraints.Unbounded<T> _ = default)
    {
        return EqualityComparer<T>.Default.Equals(left!, right!);
    }
#endregion

#region Equate(ROSpan, ROSpan)
    [OverloadResolutionPriority(100)]
    public static bool Equate<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        TypeConstraints.HasIEquatable<T> _ = default)
        where T : IEquatable<T>
    {
        return MemoryExtensions.SequenceEqual<T>(left, right);
    }

    [OverloadResolutionPriority(75)]
    public static bool Equate<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IEqualityComparer<T>? itemComparer,
        TypeConstraints.HasIEquatable<T> _ = default)
        where T : IEquatable<T>
    {
        return MemoryExtensions.SequenceEqual<T>(left, right, itemComparer);
    }

    [OverloadResolutionPriority(50)]
    public static bool Equate<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IEqualityComparer<T>? itemComparer)
    {
#if NET6_0_OR_GREATER
        return MemoryExtensions.SequenceEqual<T>(left, right, itemComparer);
#else
        int count = left.Length;
        if (right.Length != count)
            return false;
        
        if (itemComparer is not null)
        {
            for (var i = 0; i < count; i++)
            {
                if (!itemComparer.Equals(left[i], right[i]))
                    return false;
            }
        }
        else
        {
            for (var i = 0; i < count; i++)
            {
                if (!EqualityComparer<T>.Default.Equals(left[i], right[i]))
                    return false;
            }
        }
        return true;
#endif
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(50)]
    public static bool Equate<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IEqualityComparer<ReadOnlySpan<T>>? comparer)
    {
        if (comparer is not null)
        {
            return comparer.Equals(left, right);
        }
        else
        {
            return Equate<T>(left, right);
        }
    }
#endif


    [OverloadResolutionPriority(45)]
    public static bool Equate<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right)
    {
#if NET6_0_OR_GREATER
        return MemoryExtensions.SequenceEqual<T>(left, right);
#else
        int count = left.Length;
        if (right.Length != count)
            return false;
        
        for (var i = 0; i < count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(left[i], right[i]))
                return false;
        }
        return true;
#endif
    }
#endregion

}