#if !NET9_0_OR_GREATER

namespace ScrubJay.Universal;

[PublicAPI]
public sealed class AnyComparer<T> : IEqualityComparer<T>, IComparer<T>
{
    public static AnyComparer<T> Default { get; } = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(T? x, T? y) => EqualityComparer<T>.Default.Equals(x!, y!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode([DisallowNull] T value) => EqualityComparer<T>.Default.GetHashCode(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(T? x, T? y) => Comparer<T>.Default.Compare(x!, y!);
}


#endif