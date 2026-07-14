namespace ScrubJay.Universal;

public static partial class Any
{
    public static int Compare<T>(T? left, T? right)
        where T : IComparable<T>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
        {
            return left.CompareTo(right);
        }
        else if (right is not null)
        {
            return -(right.CompareTo(left));
        }
        else
        {
            return 0;
        }
    }
}