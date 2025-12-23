#if NET9_0_OR_GREATER


namespace ScrubJay.Universal;

[PublicAPI]
public sealed class AnyComparer<T> : IEqualityComparer<T>, IComparer<T>
    where T : allows ref struct
{
    public static AnyComparer<T> Default { get; } = new();

    public bool Equals(T? x, T? y)
    {
        return Any.Equals<T>(x, y);
    }

    public int GetHashCode([DisallowNull] T obj)
    {
        return Any.GetHashCode<T>(obj);
    }

    public int Compare(T? x, T? y)
    {
        return Any.CompareTo<T>(x, y);
    }
}
#endif