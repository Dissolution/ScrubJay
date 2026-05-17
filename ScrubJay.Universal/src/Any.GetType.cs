// ReSharper disable MethodOverloadWithOptionalParameter
namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Gets the <see cref="Type"/> of the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to return the <see cref="Type"/> of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with,
    /// which may be a subtype of the <paramref name="instance"/>'s actual type.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s <see cref="Type"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? instance)
    {
        if (instance is null)
            return typeof(T);
        return instance.GetType();
    }

#if NET9_0_OR_GREATER
    /// <summary>
    /// Gets the <see cref="Type"/> of the <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to return the <see cref="Type"/> of.
    /// </param>
    /// <param name="_">
    /// A <see cref="TypeConstraints"/> applied so that this method is only called with <see langword="ref struct"/> <paramref name="instance"/>s.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with,
    /// which may be a subtype of the <paramref name="instance"/>'s actual type.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s <see cref="Type"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? instance, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return typeof(T);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(scoped Span<T> span)
    {
        return typeof(Span<T>);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(scoped ReadOnlySpan<T> span)
    {
        return typeof(ReadOnlySpan<T>);
    }
}