using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    public static bool HasTryFormat<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        => TryFormatCache<T>.HasTryFormat;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFormat<T>(
        in T? instance,
        Span<char> destination,
        out int charsWritten,
        scoped text format = default,
        IFormatProvider? provider = null)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (instance is null)
        {
            charsWritten = 0;
            return true;
        }

        return TryFormatCache<T>.TryFormat(in instance, destination, out charsWritten, format, provider);
    }

    private static class TryFormatCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate bool AnyTryFormat(ref readonly T instance, Span<char> destination, out int charsWritten, scoped text format, IFormatProvider? provider);


        internal static readonly AnyTryFormat TryFormat;
        internal static readonly bool HasTryFormat;

        static TryFormatCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(ISpanFormattable.TryFormat), typeof(bool), [typeof(Span<char>), typeof(int).MakeByRefType(), typeof(text), typeof(IFormatProvider)])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyTryFormat>($"Any_{instanceType}_Format");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Ldarg_3);
                gen.Emit(OpCodes.Ldarg, 4);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyTryFormat>(out var func))
                {
                    TryFormat = func;
                    HasTryFormat = true;
                    return;
                }
            }

            TryFormat = FallbackTryFormat;
            HasTryFormat = false;
        }

        private static bool FallbackTryFormat(ref readonly T instance, Span<char> destination, out int charsWritten, scoped text format, IFormatProvider? provider)
        {
            string str = ToString<T>(in instance)!;
            charsWritten = str.Length;
            if (str.TryCopyTo(destination))
            {
                return true;
            }

            charsWritten = 0;
            return false;
        }
    }
}