using JetBrains.Annotations;

namespace ScrubJay.Functional;

[PublicAPI]
public static class Prelude
{
    public static bool Equate<T>(T? left, T? right)
        where T : IEquatable<T>
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
            return true; // both are null
        }
    }

    public static int Compare<T>(T? left, T? right)
        where T : IComparable<T>
    {
        if (left is null)
        {
            if (right is null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else if (right is null)
        {
            return 1;
        }
        else
        {
            return left.CompareTo(right);
        }
    }
}