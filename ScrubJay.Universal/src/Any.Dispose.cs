using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Dispose<T>(ref T? instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is not null)
            DisposeCache<T>.Invoke(ref instance);
    }

    private static class DisposeCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate void AnyDispose(ref T instance);

        internal static readonly AnyDispose Invoke;

        static DisposeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType.FindMatchingInstanceMethods("Dispose", typeof(void), [])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyDispose>($"Any_{instanceType}_Dispose");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyDispose>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }

            Invoke = Fallback;
        }

        private static void Fallback(ref T instance)
        {
            // do nothing
        }
    }
}