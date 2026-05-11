using ScrubJay.Universal.UNPROCESSED;

namespace ScrubJay.Universal;

/// <summary>
/// An <see cref="IEqualityComparer{T}"/> and <see cref="IComparer{T}"/> that works on any generic type.
/// </summary>
/// <typeparam name="T">
/// The <see cref="Type"/> of values to equate or compare, <i>may</i> be a <c>ref struct</c>.
/// </typeparam>
[PublicAPI]
public sealed class UniversalComparer<T> : IEqualityComparer<T>, IComparer<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    /// <summary>
    /// Gets the default <see cref="UniversalComparer{T}"/>.
    /// </summary>
    public static UniversalComparer<T> Default { get; } = new UniversalComparer<T>();

    private UniversalComparer() { }

    /// <inheritdoc cref="Any.Equals{T}(in T, in T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(T? x, T? y)
    {
        return Any.Equals<T>(in x, in y);
    }

    /// <inheritdoc cref="Any.GetHashCode{T}(in T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(T obj)
    {
        return Any.GetHashCode<T>(in obj);
    }

    /// <inheritdoc cref="Any.Compare{T}(in T, in T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(T? x, T? y)
    {
        return Any.Compare(in x, in y);
    }
}