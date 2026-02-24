namespace ScrubJay.Universal;

/// <summary>
/// An <see cref="IEqualityComparer{T}"/> and <see cref="IComparer{T}"/> that works on any generic type.
/// </summary>
/// <typeparam name="T">
/// The <see cref="Type"/> of values to equate or compare, <i>may</i> be a <c>ref struct</c>.
/// </typeparam>
[PublicAPI]
public sealed class AnyComparer<T> : IEqualityComparer<T>, IComparer<T>
#if NET9_0_OR_GREATER
    where T : allows ref struct
#endif
{
    /// <summary>
    /// Gets the default <see cref="AnyComparer{T}"/>.
    /// </summary>
    public static AnyComparer<T> Default { get; } = new AnyComparer<T>();

    private AnyComparer() { }

    /// <inheritdoc cref="Any.Equals{T}(T,T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(T? x, T? y)
    {
        return Any.Equals<T>(x, y);
    }

    /// <inheritdoc cref="Any.GetHashCode{T}(T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetHashCode(T obj)
    {
        return Any.GetHashCode<T>(obj);
    }

    /// <inheritdoc cref="Any.Compare{T}(T,T)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int Compare(T? x, T? y)
    {
        return Any.Compare<T>(x, y);
    }
}