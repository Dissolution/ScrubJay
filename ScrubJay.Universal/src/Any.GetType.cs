// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(T? value)
    {
        return value?.GetType() ?? typeof(T);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return typeof(T);
    }
}
#endif