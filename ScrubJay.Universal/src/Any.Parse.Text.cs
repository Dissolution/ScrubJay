// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
#if NET7_0_OR_GREATER
    public static bool TryParse<T>(
        text text,
        [MaybeNullWhen(false)] out T instance)
        where T : ISpanParsable<T>
    {
        return T.TryParse(text, null, out instance);
    }
#endif

#if NET7_0_OR_GREATER
    public static bool TryParse<T>(
        text text,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance)
        where T : ISpanParsable<T>
    {
        return T.TryParse(text, provider, out instance);
    }
#endif

    public static bool HasTryParseText<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TryParseTextCache<T>.HasInvoke;
    }

    public static bool TryParse<T>(
        text text,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TryParseTextCache<T>.Invoke(text, null, out instance);
    }

    public static bool TryParse<T>(
        text text,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out T instance,
        TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return TryParseTextCache<T>.Invoke(text, provider, out instance);
    }


    private static class TryParseTextCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal static readonly bool HasInvoke;
        internal static readonly AnyTryParseText<T> Invoke;

        static TryParseTextCache()
        {
            Type type = typeof(T);
            MethodInfo? method = type
                .FindMatchingMethods(
                    "TryParse",
                    typeof(bool),
                    [typeof(text), typeof(IFormatProvider), type.MakeByRefType()])
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyTryParseText<T>>(
                $"Any_{type}_TryParse_Text",
                gen => gen
                    .Ldarg(0)
                    .Ldarg(1)
                    .Ldarg(2)
                    .Call(method)
                    .Ret(), out Invoke!))
            {
                HasInvoke = true;
                return;
            }

            HasInvoke = false;
            Invoke = FallbackTryParseText;
        }

        private static bool FallbackTryParseText(
            text text,
            IFormatProvider? provider,
            [MaybeNullWhen(false)] out T instance)
        {
            instance = default;
            return false;
        }
    }
}