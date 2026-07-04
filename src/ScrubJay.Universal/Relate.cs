namespace ScrubJay.Universal;

[PublicAPI]
public static class Relate
{
    public static bool Equate<T>(T? left, T? right)
        where T : IEquatable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
            return left.Equals(right!);
        if (right is not null)
            return right.Equals(left!);
        return true;
    }
    
    public static new bool Equals(object? left, object? right)
    {
        return object.Equals(left, right);
    }
    
    public static bool Equals<T>(T? left, T? right)
    {
        return EqualityComparer<T>.Default.Equals(left!, right!);
    }

    public static int Compare<T>(T? left, T? right)
        where T : IComparable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
            return left.CompareTo(right!);
        if (right is not null)
            return -(right.CompareTo(left!));
        return 0;
    }
}