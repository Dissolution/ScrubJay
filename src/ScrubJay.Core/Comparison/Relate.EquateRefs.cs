namespace ScrubJay.Comparison;

public static partial class Relate
{
    public static bool EquateRefs(object? left, object? right)
    {
        return object.ReferenceEquals(left, right);
    }

    public static bool EquateRefs<T>(ref readonly T left, ref readonly T right)
    {
        return Unsafe.AreSame(in left, in right);
    }
}