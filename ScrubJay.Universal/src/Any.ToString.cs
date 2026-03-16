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
    public static string? ToString<T>(ref readonly T? value)
    {
        return value?.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(ref readonly ReadOnlySpan<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(ref readonly Span<T> span)
    {
        return span.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString(ref readonly text text)
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
    public static string? ToString<T>(ref readonly T? value, TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return null;
        return ToStringCache<T>.Invoke(in value);
    }

    private static class ToStringCache<T>
        where T : allows ref struct
    {
        public delegate string AnyToString(ref readonly T value);

        public static readonly AnyToString Invoke;

        static ToStringCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? toStringMethod = instanceType.FindBestMethod<AnyToString>(nameof(object.ToString));

            if (toStringMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyToString>($"Any_{Type.Render<T>()}_ToString");
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, toStringMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyToString>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }
            Invoke = (ref readonly _) => $"{Type.Render<T>()} instance";
        }
    }
}
#endif