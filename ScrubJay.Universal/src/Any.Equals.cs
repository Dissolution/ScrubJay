#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Universal;

partial class Any
{
    /*public static bool Equal<T>(in T? leftR, in T? rightR)
    {

    }

    public static bool Equal<T>(ref readonly T? left, ref readonly T? right)
    {
        return EqualityComparer<T>.Default.Equals(left, right);
    }

    public static bool Equal<E>(ref readonly E? left, ref readonly E? right, TypeConstraints.HasIEquatable<E> _ = default)
        where E : IEquatable<E>
    {
        if (left is not null)
            return left.Equals(right);
        if (right is not null)
            return right.Equals(left);
        return ReferenceEquals(left, right);
    }

    public static bool Equal<E>(ref readonly E? left, ref readonly E? right, TypeConstraints.HasIEqualityOperators<E> _ = default)
        where E : IEqualityOperators<E, E, bool>
    {
        return left == right;
    }*/


    /// <summary>
    /// Determines whether two <typeparamref name="T"/> values are equal.
    /// </summary>
    /// <param name="left">
    /// The first <typeparamref name="T"/> value to equate.
    /// </param>
    /// <param name="right">
    /// The second <typeparamref name="T"/> value to equate.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to equate.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the values are equal; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? left, T? right)
        => EqualityComparer<T>.Default.Equals(left!, right!);

    /// <summary>
    /// Use an <see cref="IEqualityComparer{T}"/> to determine whether two <typeparamref name="T"/> values are equal.
    /// </summary>
    /// <param name="left">
    /// The first <typeparamref name="T"/> value to equate.
    /// </param>
    /// <param name="right">
    /// The second <typeparamref name="T"/> value to equate.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> that will determine equality.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to equate.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the values are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(in T? left, T? right, IEqualityComparer<T>? comparer)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (comparer is null)
        {
#if NET9_0_OR_GREATER
            return Equals(in left, right);
#else
            return Equals(left, right);
#endif
        }
        return comparer.Equals(left!, right!);
    }

    /// <summary>
    /// Determines whether two <see cref="ReadOnlySpan{T}"/>s are equal.
    /// </summary>
    /// <param name="left">
    /// The first <see cref="ReadOnlySpan{T}"/> to equate.
    /// </param>
    /// <param name="right">
    /// The second <see cref="ReadOnlySpan{T}"/> to equate.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of items in the <see cref="ReadOnlySpan{T}"/>s.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the sequences are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(scoped ReadOnlySpan<T> left, scoped ReadOnlySpan<T> right)
#if !NET10_0_OR_GREATER
        where T : IEquatable<T>
#endif
    {
        return MemoryExtensions.SequenceEqual(left, right);
    }

    /// <summary>
    /// Determines whether two <see cref="ReadOnlySpan{T}"/>s are equal by comparing their items with an <see cref="IEqualityComparer{T}"/>.
    /// </summary>
    /// <param name="left">
    /// The first <see cref="ReadOnlySpan{T}"/> to equate.
    /// </param>
    /// <param name="right">
    /// The second <see cref="ReadOnlySpan{T}"/> to equate.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> to use when comparing items,
    /// or <see langword="null"/> to use <see cref="EqualityComparer{T}.Default"/>.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of items in the <see cref="ReadOnlySpan{T}"/>s.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the sequences are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IEqualityComparer<T>? comparer)
    {
#if !NETSTANDARD && !NETFRAMEWORK
        return MemoryExtensions.SequenceEqual(left, right, comparer);
#else
        // If the spans differ in length, they're not equal.
        if (left.Length != right.Length)
        {
            return false;
        }

        // Use the comparer to compare each element.
        comparer ??= EqualityComparer<T>.Default;
        for (int i = 0; i < left.Length; i++)
        {
            if (!comparer.Equals(left[i], right[i]))
            {
                return false;
            }
        }

        return true;
#endif
    }

    /// <summary>
    /// Determines if two <see cref="ReadOnlySpan{char}"/> texts have the same <see cref="StringComparison.Ordinal">Ordinal</see> characters.
    /// </summary>
    /// <param name="left">
    /// The first <see cref="text"/> to equate.
    /// </param>
    /// <param name="right">
    /// The second <see cref="text"/> to equate.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the texts are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals(scoped text left, scoped text right)
    {
        return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Determines if two <see cref="ReadOnlySpan{char}"/> texts have the same characters according to a <see cref="StringComparison"/>.
    /// </summary>
    /// <param name="left">
    /// The first <see cref="text"/> to equate.
    /// </param>
    /// <param name="right">
    /// The second <see cref="text"/> to equate.
    /// </param>
    /// <param name="comparison">
    /// A <see cref="StringComparison"/> that determines how the texts are compared.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the texts are equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool Equals(scoped text left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.Equals(left, right, comparison);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Determines whether two <typeparamref name="T"/> values are equal.
    /// </summary>
    /// <param name="left">
    /// The first <typeparamref name="T"/> value to equate.
    /// </param>
    /// <param name="right">
    /// The second <typeparamref name="T"/> value to equate.
    /// </param>
    /// <param name="_">
    /// Ignored <see cref="TypeConstraints"/> on <typeparamref name="T"/> that assists with method overload resolution.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to equate, may be a <c>ref struct</c>.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if the values are equal; otherwise <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(ref readonly T? left, T? right,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return EqualsCache<T>.Invoke(in left, right);
    }

    private static class EqualsCache<T>
        where T : allows ref struct
    {
        public delegate bool AnyEquals(ref readonly T? value, T? other);

        public static readonly AnyEquals Invoke;

        static EqualsCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? equalsMethod = instanceType.FindBestMethod("Equals", typeof(bool), [typeof(T)]);

            if (equalsMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyEquals>($"Any_{Type.Render<T>()}_Equals");
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, equalsMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyEquals>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }
            Invoke = EqualsFallback;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsFallback(ref readonly T? left, T? right)
        {
            Emit.Ldarg_0();
            Emit.Ldobj<T>();
            Emit.Ldarg_1();
            Emit.Ceq();
            return Return<bool>();
        }
    }

}
#endif