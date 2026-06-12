using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

public partial class Any
{
    /// <summary>
    /// Returns a <see cref="string"/> representation of the <typeparamref name="T"/> <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to return the <see cref="string"/> representation of.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s <see cref="string"/> representation.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(instance))]
#if !NET9_0_OR_GREATER
    public static string? ToString<T>(in T? instance)
    {
        if (instance is null)
            return null;
        return instance.ToString()!;
    }
#else
    public static string? ToString<T>(in T? instance)
        where T : allows ref struct
    {
        if (instance is null)
            return null;
        return ToStringCache<T>.Invoke(in instance);
    }
#endif

    private static class ToStringCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        internal delegate string AnyToString(in T value);

        internal static readonly AnyToString Invoke;

        static ToStringCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods("ToString", typeof(string), Type.EmptyTypes)
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = CreateDynamicMethod<AnyToString>($"Any_{instanceType}_ToString");
                var gen = dynamicMethod.GetILGenerator();

                gen.Emit(OpCodes.Ldarg_0);
                gen.Emit(OpCodes.Constrained, instanceType);
                gen.Emit(OpCodes.Callvirt, method);
                gen.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyToString>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }

            Invoke = Fallback;
        }

        private static string Fallback(in T instance)
            => typeof(T).ToString(); // same as object.ToString()
    }
}