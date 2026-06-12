using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        [MaybeNullWhen(false)] out T instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TryParseSpanCache<T>.Invoke(text, null, out instance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryParse<T>(
        scoped text text,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TryParseSpanCache<T>.Invoke(text, provider, out instance);
    }


    private static class TryParseSpanCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate bool TryParseSpan(scoped text text, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance);

        internal static volatile TryParseSpan Invoke;

        static TryParseSpanCache()
        {
            Type type = typeof(T);
            MethodInfo? method = type
                .FindMatchingStaticMethods("TryParse",
                    typeof(bool),
                    [typeof(text), typeof(IFormatProvider), type.MakeByRefType()])
                .FirstOrDefault();

            if (method is not null)
            {
                Debug.Assert(method.IsStatic);

                var dynamicMethod = CreateDynamicMethod<TryParseSpan>($"{type}_TryParseSpan");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Call, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<TryParseSpan>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }

            Invoke = Fallback;
        }

        private static bool Fallback(scoped text text, IFormatProvider? provider, [MaybeNullWhen(false)] out T instance)
        {
            instance = default;
            return false;
        }
    }
}