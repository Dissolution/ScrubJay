using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(in T? instance, in T? other)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
            return other is null;
        return EqualsCache<T>.Invoke(in instance, in other);
    }

    private static class EqualsCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate bool AnyEquals(in T instance, in T? other);

        internal static readonly AnyEquals Invoke;

        static EqualsCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods("Equals", typeof(bool), [instanceType])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyEquals>($"Any_{instanceType}_Equals");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldobj, instanceType);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyEquals>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }

            Invoke = Fallback;
        }

        private static bool Fallback(in T instance, in T? other)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Ceq();
            return Return<bool>();
        }
    }
}