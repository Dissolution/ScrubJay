namespace ScrubJay.Functional.Utilities;

/// <summary>
/// Static utility class to assist with common method calls related to Equality and Comparison
/// </summary>
[PublicAPI]
public static class Relate
{
    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    public static bool Equal<E>(E? left, E? right)
        where E : IEquatable<E>
    {
        if (left is not null)
            return left.Equals(other: right!);
        if (right is not null)
            return right.Equals(other: left!);
        return true;
    }

    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    public static bool Equal<E>(E? left, E? right, IEqualityComparer<E> equalityComparer)
    {
        return equalityComparer.Equals(left!, right!);
    }

    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    public static int Compare<C>(C? left, C? right)
        where C : IComparable<C>
    {
        if (left is not null)
            return left.CompareTo(other: right!);
        if (right is not null)
            return -(right.CompareTo(other: left!));
        return 0;
    }

    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    public static int Compare<C>(C? left, C? right, IComparer<C> comparer)
    {
        return comparer.Compare(left!, right!);
    }

    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    public static bool EqualTo<EI, EO>(EI? instance, EO? other)
        where EI : IEquatable<EO>
    {
        if (instance is not null)
            return instance.Equals(other: other!);
        return other is null;
    }

    [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
    public static int CompareTo<CI, CO>(CI? instance, CO? other)
        where CI : IComparable<CO>
    {
        if (instance is not null)
            return instance.CompareTo(other!);
        if (other is null)
            return 0;
        return -1; // null sorts before non-null
    }
}