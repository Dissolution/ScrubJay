#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Determines whether a <typeparamref name="T"/> <paramref name="value"/> is equal to an <see cref="object"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="other">
    /// The <see cref="object"/> to compare.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> to compare to the <see cref="object"/>.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="value"/> and <see cref="object"/> are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(in T? value, object? other)
    {
        if (value is null)
            return other is null;
        return value.Equals(other);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Determines whether a <typeparamref name="T"/> <paramref name="value"/> is equal to an <see cref="object"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="other">
    /// The <see cref="object"/> to compare.
    /// </param>
    /// <param name="_"></param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> to compare to the <see cref="object"/>.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="value"/> and <see cref="object"/> are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(
        in T? value,
        object? other,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return other is null;
        return EqualsObjectCache<T>.Invoke(in value, other);
    }

    private static class EqualsObjectCache<T>
        where T : allows ref struct
    {
        public delegate bool AnyEqualsObject(ref readonly T? value, object? other);

        public static readonly AnyEqualsObject Invoke;

        static EqualsObjectCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? equalsMethod = instanceType.FindBestMethod("Equals", typeof(bool), [typeof(object)]);

            if (equalsMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyEqualsObject>($"Any_{instanceType}_Equals_Object");
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, equalsMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyEqualsObject>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }
            Invoke = (ref readonly value, other) => false;
        }
    }
}

#endif