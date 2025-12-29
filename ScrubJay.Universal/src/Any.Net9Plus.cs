// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

#if NET9_0_OR_GREATER
static partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CompareTo<T>(T? value, T? other, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.CompareTo(value, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? value, object? obj, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.Equals(value, obj);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? value, T? other, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.Equals(value, other);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.GetHashCode(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.GetType(value);
    }
}
#endif