using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
// ReSharper disable MethodOverloadWithOptionalParameter

namespace ScrubJay.Universal;

partial class Any
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
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? ToString<T>(in T? instance)
    {
        if (instance is null)
            return null;
        return instance.ToString()!;
    }

#if NET9_0_OR_GREATER
    /// <summary>
    /// Returns a <see cref="string"/> representation of the <typeparamref name="T"/> <paramref name="instance"/>.
    /// </summary>
    /// <param name="instance">
    /// The instance to return the <see cref="string"/> representation of.
    /// </param>
    /// <param name="_">
    /// A <see cref="TypeConstraints"/> applied so that this method is only called with <see langword="ref struct"/> <paramref name="instance"/>s.
    /// </param>
    /// <typeparam name="T">
    /// The generic <see cref="Type"/> this method was called with.
    /// </typeparam>
    /// <returns>
    /// The <paramref name="instance"/>'s <see cref="string"/> representation.
    /// </returns>
    [return: NotNullIfNotNull(nameof(instance))]
    public static string? ToString<T>(in T? instance, TypeConstraints.AllowsRefStruct<T> _ = default)
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
        internal delegate string AnyToString(ref readonly T value);

        internal static volatile AnyToString Invoke;

        static ToStringCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? method = instanceType
                .FindMatchingInstanceMethods("ToString", typeof(string), Type.EmptyTypes)
                .FirstOrDefault();

            if (method is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyToString>($"Any_{instanceType}_ToString");
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
        
        private static string Fallback(ref readonly T value)
            => typeof(T).ToString(); // same as object.ToString()
    }
}