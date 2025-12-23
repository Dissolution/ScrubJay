// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

#if NET9_0_OR_GREATER
static partial class Any
{
    public static string ToString<T>(T? value, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.ToString(value);
    }

    public static int CompareTo<T>(T? value, T? other, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.CompareTo(value, other);
    }
    
    public static bool Equals<T>(T? value, object? obj, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.Equals(value, obj);
    }
    
    public static bool Equals<T>(T? value, T? other, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.Equals(value, other);
    }

    public static int GetHashCode<T>(T? value, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.GetHashCode(value);
    }
    
    public static Type GetType<T>(T? value, AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return Any<T>.GetType(value);
    }
}
#endif