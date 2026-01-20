// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Gets the <see cref="Type"/> of the given <paramref name="value"/>
    /// </summary>
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
    /// Gets the <see cref="Type"/> of the given <paramref name="value"/>
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return typeof(T);
    }
}
#endif