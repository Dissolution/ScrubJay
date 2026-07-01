namespace ScrubJay.Universal;

[PublicAPI]
public static class Relate
{
    public static bool Equate<T>(T? left, T? right)
    {
        return EqualityComparer<T>.Default.Equals(left!, right!);
    }

    public static int Compare<T>(T? left, T? right)
        where T : IComparable<T>
    {
        if (left is not null)
            return left.CompareTo(right!);
        if (right is not null)
            return -(right.CompareTo(left!));
        return 0;
    }
}