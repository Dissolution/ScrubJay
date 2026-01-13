namespace ScrubJay.Universal;

[PublicAPI]
public sealed class AnyComparer<T> : IEqualityComparer<T>, IComparer<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    public static AnyComparer<T> Default { get; } = new AnyComparer<T>();

    private AnyComparer() { }

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
        return Any.Compare<T>(x, y);
    }
}