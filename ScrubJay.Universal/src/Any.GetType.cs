// ReSharper disable MethodOverloadWithOptionalParameter

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)
namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Gets the <see cref="Type"/> of a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to get the true <see cref="Type"/> of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> of this method invocation, which may be less specific than the true <see cref="Type"/>.
    /// </typeparam>
    /// <returns>
    /// The true <see cref="Type"/> of <paramref name="value"/>, which may be more specific than <c>typeof(T)</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(T? value)
    {
        if (value is not null)
        {
            return value.GetType();
        }

        return typeof(T);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Gets the <see cref="Type"/> of a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to get the true <see cref="Type"/> of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> of this method invocation.
    /// </typeparam>
    /// <returns><c>typeof(T)</c></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return typeof(T);
    }
}
#endif