using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            return false;
        }

        return instance.TryFormat(destination, out charsWritten, format, provider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFormat<T>(
        in T? instance,
        Span<char> destination,
        out int charsWritten,
        scoped text format = default,
        IFormatProvider? provider = null,
        TypeConstraints.IsUnconstrained<T> _ = default)
    {
        if (instance is null)
        {
            charsWritten = 0;
            return false;
        }

        return TryFormatCache<T>.Invoke(in instance, destination, out charsWritten, format, provider);
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFormat<T>(
        in T? instance,
        Span<char> destination,
        out int charsWritten,
        scoped text format = default,
        IFormatProvider? provider = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (instance is null)
        {
            charsWritten = 0;
            return false;
        }

        return TryFormatCache<T>.Invoke(in instance, destination, out charsWritten, format, provider);
    }
#endif

    private static class TryFormatCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        private delegate bool AnyTryFormat(ref readonly T instance, Span<char> destination, out int charsWritten, scoped text format, IFormatProvider? provider);

        private static volatile AnyTryFormat _delegate;
        private static volatile bool _delegateTested;

        static TryFormatCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(ISpanFormattable.TryFormat), typeof(bool), [typeof(Span<char>), typeof(int).MakeByRefType(), typeof(text), typeof(IFormatProvider)])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyTryFormat>($"Any_{instanceType}_Format");
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
                    _delegate = func;
                    _delegateTested = false;
                    return;
                }
            }

            _delegate = Fallback;
            _delegateTested = true;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool Fallback(ref readonly T instance, Span<char> destination, out int charsWritten, scoped text format, IFormatProvider? provider)
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryInvoke(ref readonly T instance, Span<char> destination, out int charsWritten, scoped text format, IFormatProvider? provider)
        {
            try
            {
                return _delegate(in instance, destination, out charsWritten, format, provider);
            }
            catch
            {
                _delegate = Fallback;
                return _delegate(in instance, destination, out charsWritten, format, provider);
            }
            finally
            {
                _delegateTested = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Invoke(ref readonly T instance, Span<char> destination, out int charsWritten, scoped text format, IFormatProvider? provider)
        {
            if (_delegateTested)
                return _delegate(in instance, destination, out charsWritten, format, provider);
            return TryInvoke(in instance, destination, out charsWritten, format, provider);
        }
    }
}