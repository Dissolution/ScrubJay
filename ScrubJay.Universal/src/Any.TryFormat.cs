// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
#if NET6_0_OR_GREATER
    public static bool TryFormat<T>(
        in T? instance,
        Span<char> destination,
        out int charsWritten,
        text format = default,
        IFormatProvider? provider = null)
        where T : ISpanFormattable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (instance is null)
        {
            charsWritten = 0;
            return true;
        }

        return instance.TryFormat(destination, out charsWritten, format, provider);
    }
#endif

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
        text format = default,
        IFormatProvider? provider = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
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
        internal static readonly AnyTryFormat<T> TryFormat;
        internal static readonly bool HasTryFormat;

        static TryFormatCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingMethods("TryFormat",
                    returnType: typeof(bool),
                    parameterTypes: [typeof(Span<char>), typeof(int).MakeByRefType(), typeof(text), typeof(IFormatProvider)])
                .FirstOrDefault();

            if (method is not null && DynamicMethod.TryGenerateDelegate<AnyTryFormat<T>>(
                $"Any_{instanceType}_TryFormat",
                gen => gen
                    .Ldarg(0)
                    .Ldarg(1)
                    .Ldarg(2)
                    .Ldarg(3)
                    .Ldarg(4)
                    .Constrained(instanceType)
                    .Callvirt(method)
                    .Ret(), out TryFormat!))
            {
                HasTryFormat = true;
                return;
            }


            TryFormat = FallbackTryFormat;
            HasTryFormat = false;
        }

        private static bool FallbackTryFormat(in T? instance, Span<char> destination, out int charsWritten, scoped text format, IFormatProvider? provider)
        {
            string str = Any.ToString<T>(in instance)!;
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