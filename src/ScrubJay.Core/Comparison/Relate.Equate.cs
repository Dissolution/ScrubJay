
using ScrubJay.Generics;

namespace ScrubJay.Comparison;

public static partial class Relate
{
    public static bool Equate(scoped text left, scoped text right)
    {
        return left.Equals(right, StringComparison.Ordinal);
    }

    public static bool Equate(scoped text left, scoped text right, StringComparison comparison)
    {
        return left.Equals(right, comparison);
    }


    public static bool Equate<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
    {
        return left.SequenceEqual(right);
    }

    public static bool Equate<T>(ReadOnlySpan<T> left, ReadOnlySpan<T> right, IEqualityComparer<T>? itemComparer)
    {
        return left.SequenceEqual(right, itemComparer);
    }


    [OverloadResolutionPriority(100)]
    public static bool Equate<T>(T? left, T? right, Constrain.HasIEquatable<T> _ = default)
        where T : IEquatable<T>
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(95)]
    public static bool Equate<T>(T? left, T? right, Constrain.AllowsRefStruct.HasIEquatable<T> _ = default)
        where T : IEquatable<T>, allows ref struct
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }
#endif

    [OverloadResolutionPriority(90)]
    public static bool Equate<L, R>(L? left, R? right)
        where L : IEquatable<R>
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(85)]
    public static bool Equate<L, R>(L? left, R? right, Constrain.AllowsRefStruct.HasIEquatable<L,R> _ = default)
        where L : IEquatable<R>, allows ref struct
        where R : allows ref struct
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }
#endif

    [OverloadResolutionPriority(60)]
    public static bool Equate<T>(T? left, T? right, Constrain.Unbounded<T> _ = default)
    {
        return EqualityComparer<T>.Default.Equals(left, right);
    }

    [OverloadResolutionPriority(55)]
    public static bool Equate<T>(T? left, T? right, Constrain.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        //return Anything.Equals(in left, in right);
        throw new NotImplementedException();
    }

    [OverloadResolutionPriority(40)]
    public static bool Equate<T>(T? instance, object? other)
    {
        if (instance is null)
            return other is null;
        return instance.Equals(other);
    }

#if NET9_0_OR_GREATER
    [OverloadResolutionPriority(35)]
    public static bool Equate<T>(in T? instance, object? other)
        where T : allows ref struct
    {
        return Anything.Equals<T>(in instance, other);
    }
#endif

    [OverloadResolutionPriority(20)]
    public static bool Equate(object? left, object? right)
    {
        // ReSharper disable once RedundantNameQualifier
        return object.Equals(left, right);
    }
}