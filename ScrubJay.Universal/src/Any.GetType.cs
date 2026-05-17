// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Type GetType<T>(in T? instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
            return typeof(T);
        return GetTypeCache<T>.Invoke(in instance);
    }
    
    private static class GetTypeCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate Type AnyGetType(in T instance);

        internal static readonly AnyGetType Invoke;

        static GetTypeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType.FindMatchingInstanceMethods("GetType", typeof(Type), [])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyGetType>($"Any_{instanceType}_GetType");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyGetType>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }

            Invoke = Fallback;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Type Fallback(in T instance) => typeof(T);
    }
}