using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? Format<F>(
        in F? instance,
        string? format = null,
        IFormatProvider? provider = null)
        where F : IFormattable
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (instance is null)
            return null;
        return instance.ToString(format, provider);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? Format<T>(
        in T? instance,
        string? format = null,
        IFormatProvider? provider = null,
        TypeConstraints.IsUnconstrained<T> _ = default)
    {
        if (instance is null)
            return null;
        return FormatCache<T>.Invoke(in instance, format, provider);
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? Format<T>(
        in T? instance,
        string? format = null,
        IFormatProvider? provider = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (instance is null)
            return null;
        return FormatCache<T>.Invoke(in instance, format, provider);
    }
#endif

    private static class FormatCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        private delegate string AnyFormat(ref readonly T instance, string? format, IFormatProvider? provider);

        private static volatile AnyFormat _delegate;
        private static volatile bool _delegateTested;

        static FormatCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods(nameof(IFormattable.ToString), typeof(string), [typeof(string), typeof(IFormatProvider)])
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyFormat>($"Any_{instanceType}_Format");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldarg_2);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyFormat>(out var func))
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
        private static string Fallback(ref readonly T instance, string? format, IFormatProvider? provider)
        {
            return Any.ToString<T>(in instance)!;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string TryInvoke(ref readonly T instance, string? format, IFormatProvider? provider)
        {
            try
            {
                return _delegate(in instance, format, provider);
            }
            catch
            {
                _delegate = Fallback;
                return _delegate(in instance, format, provider);
            }
            finally
            {
                _delegateTested = true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Invoke(ref readonly T instance, string? format, IFormatProvider? provider)
        {
            if (_delegateTested)
                return _delegate(in instance, format, provider);
            return TryInvoke(in instance, format, provider);
        }
    }
}