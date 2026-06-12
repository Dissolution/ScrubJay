using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? instance, in T? other)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is not null)
            return CompareCache<T>.Invoke(in instance, in other);

        if (other is not null)
            return -1;

        return 0;
    }

    private static class CompareCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate int AnyCompare(ref readonly T left, ref readonly T? right);

        internal static readonly AnyCompare Invoke;

        static CompareCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType.FindMatchingInstanceMethods("CompareTo", typeof(int), [instanceType])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyCompare>($"Any_{instanceType}_CompareTo");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldobj, instanceType);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyCompare>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }

            Invoke = Fallback;
        }

        private static int Fallback(ref readonly T left, ref readonly T? right)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Clt();
            Emit.Brtrue("lt");
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Cgt();
            Emit.Brtrue("gt");
            Emit.Ldc_I4_0();
            Emit.Ret();
            MarkLabel("lt");
            Emit.Ldc_I4_M1();
            Emit.Ret();
            MarkLabel("gt");
            Emit.Ldc_I4_1();
            Emit.Ret();
            throw Unreachable();
        }
    }
}