#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value, string? format, IFormatProvider? formatProvider = null)
    {
        if (value is not null)
        {
            if (value is IFormattable)
            {
                return ((IFormattable)value).ToString(format, formatProvider);
            }
            return value.ToString()!;
        }
        return null;
    }
}

#if NET9_0_OR_GREATER
static partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value, 
        string? format,
        IFormatProvider? formatProvider = null,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return null;
        return FormatCache<T>.ToString(in value, format, formatProvider);
    }

    private static class FormatCache<T>
        where T : allows ref struct
    {
        private delegate string AnyFormat(ref readonly T value, string? format, IFormatProvider? formatProvider);

        private static AnyFormat _delegate;
        private static bool _delegateTested;

        static FormatCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? formatMethod = instanceType.FindBestMethod("ToString", typeof(string), [typeof(string), typeof(IFormatProvider)]);

            if (formatMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyFormat>($"Any_{instanceType}_Format");
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldarg_2);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, formatMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyFormat>(out var func))
                {
                    _delegate = func;
                    _delegateTested = false;
                    return;
                }
            }
            
            _delegate = FallbackFormat;
            _delegateTested = true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string FallbackFormat(ref readonly T value, string? format, IFormatProvider? formatProvider) 
            => $"instanceof({typeof(T)}):{format}";

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string UntestedToString(ref readonly T value, string? format, IFormatProvider? formatProvider)
        {
            try
            {
                return _delegate(in value, format,  formatProvider);
            }
            catch
            {
                _delegate = FallbackFormat;
                return _delegate(in value, format, formatProvider);
            }
            finally
            {
                _delegateTested = true;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(ref readonly T value, string? format, IFormatProvider? formatProvider)
        {
            if (_delegateTested)
            {
                return _delegate(in value, format, formatProvider);
            }
            return UntestedToString(in value, format, formatProvider);
        }
    }
}
#endif