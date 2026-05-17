using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? Format<T>(
        in T? instance,
        string? format = null,
        IFormatProvider? provider = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
            return null;
        return FormatCache<T>.Invoke(in instance, format, provider);
    }

    private static class FormatCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate string AnyFormat(ref readonly T instance, string? format, IFormatProvider? provider);

        internal static volatile AnyFormat Invoke;

        static FormatCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(IFormattable.ToString), typeof(string), [typeof(string), typeof(IFormatProvider)])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyFormat>($"Any_{instanceType}_Format");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyFormat>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }

            Invoke = Fallback;
        }

        private static string Fallback(ref readonly T instance, string? format, IFormatProvider? provider)
        {
            return ToStringCache<T>.Invoke(in instance);
        }
    }
}