#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

static partial class Any
{
    /// <summary>
    /// Returns the <see cref="string"/> representation of this <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> instance to call <see cref="object.ToString"/> on.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/>.
    /// </typeparam>
    /// <returns>
    /// <c>value?.ToString()</c>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value)
    {
        return value?.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(scoped ReadOnlySpan<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(scoped Span<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(scoped text text)
    {
        return text.ToString();
    }
}

#if NET9_0_OR_GREATER
static partial class Any
{
    /// <summary>
    /// Returns the <see cref="string"/> representation of this <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> instance to call <see cref="object.ToString"/> on.
    /// </param>
    /// <param name="_">
    /// Ignored <see cref="TypeConstraints"/> to assist in method overloading.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/>, may be a <c>ref struct</c>.
    /// </typeparam>
    /// <returns>
    /// <c>value?.ToString()</c>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(value))]
    public static string? ToString<T>(in T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return null;
        return ToStringCache<T>.ToString(in value);
    }

    private static class ToStringCache<T>
        where T : allows ref struct
    {
        public delegate string AnyToString(ref readonly T value);

        private static AnyToString _delegate;
        private static bool _delegateTested;

        static ToStringCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? toStringMethod = instanceType.FindBestMethod("ToString", typeof(string), []);

            if (toStringMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyToString>($"Any_{instanceType}_ToString");
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, toStringMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyToString>(out var func))
                {
                    _delegate = func;
                    _delegateTested = false;
                    return;
                }
            }
            
            _delegate = FallbackToString;
            _delegateTested = true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string FallbackToString(ref readonly T value) => $"instanceof({typeof(T)})";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(ref readonly T value)
        {
            if (_delegateTested)
            {
                return _delegate(in value);
            }

            return untestedInvoke(in value);
            
            static string untestedInvoke(ref readonly T value)
            {
                try
                {
                    return _delegate(in value);
                }
                catch
                {
                    _delegate = FallbackToString;
                    return _delegate(in value);
                }
                finally
                {
                    _delegateTested = true;
                }
            }
        }
    }
}
#endif