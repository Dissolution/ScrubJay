using System.Reflection;
using System.Reflection.Emit;
using ScrubJay.Universal.Reflection;
#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter
// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Universal.UNPROCESSED;

partial class Any
{
    public static int Compare<C>(in C? left, in C? right)
        where C : IComparable<C>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        if (left is not null)
            return left.CompareTo(right!);

        if (right is not null)
            return -1;

        return 0;
    }

    public static int Compare<T>(in T? left, in T? right, IComparer<T> comparer)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return comparer.Compare(left!, right!);
    }

    public static int Compare<C>(in C? left, in C? right, TypeConstraints.HasIComparable _ = default)
        where C : IComparable
    {
        if (left is not null)
            return left.CompareTo(right!);

        if (right is not null)
            return -1;

        return 0;
    }

    public static int Compare<C>(scoped ReadOnlySpan<C> left, scoped ReadOnlySpan<C> right)
        where C : IComparable<C>
    {
        return MemoryExtensions.SequenceCompareTo(left, right);
    }

    public static int Compare<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IComparer<T> comparer)
    {
#if NET10_0_OR_GREATER
        return MemoryExtensions.SequenceCompareTo(left, right, comparer);
#else
        int minLength = Math.Min(left.Length, right.Length);
        comparer ??= Comparer<T>.Default;

        for (int i = 0; i < minLength; i++)
        {
            int c = comparer.Compare(left[i], right[i]);
            if (c != 0)
            {
                return c;
            }
        }

        return left.Length.CompareTo(right.Length);
#endif
    }


    public static int Compare(scoped text left, scoped text right)
    {
        return MemoryExtensions.CompareTo(left, right, StringComparison.Ordinal);
    }

    public static int Compare(scoped text left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.CompareTo(left, right, comparison);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right,
        IComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _ = default)
    {
        if (comparer is not null)
            return comparer.Compare(left!, right!);
        return Compare<T>(left, right, _);
    }


    /// <summary>
    /// Compares two <typeparamref name="T"/> values and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="left">
    /// The first <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="right">
    /// The second <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="_">
    /// Ignored <see cref="TypeConstraints"/> on <typeparamref name="T"/> that assists with method overload resolution.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to compare, may be a <see langword="ref struct"/>.
    /// </typeparam>
    /// <returns>
    /// <c>&lt;0</c> if <paramref name="left"/> is less than <paramref name="right"/><br/>
    /// <c>0</c> if <paramref name="left"/> is equal to <paramref name="right"/> <b>or</b> if values are not comparable.<br/>
    /// <c>&gt;0</c> if <paramref name="left"/> is greater than <paramref name="right"/>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right,
        TypeConstraints.AllowsRefStruct<T> _ = default)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        return CompareCache<T>.Invoke(in left, in right);
    }

    private static class CompareCache<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        public delegate int AnyCompare(ref readonly T? left, ref readonly T? right);

        public static readonly AnyCompare Invoke;

        static CompareCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? compareToMethod = instanceType.FindMatchingInstanceMethod("CompareTo", typeof(int), typeof(T));

            if (compareToMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New($"Any_{instanceType}_Compare", typeof(int), [typeof(T), typeof(T)]);
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldobj, instanceType);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, compareToMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyCompare>(out var func))
                {
                    Invoke = func;
                    return;
                }
            }
            Invoke = CompareFallback;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CompareFallback(ref readonly T? left, ref readonly T? right)
        {
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Clt();
            Emit.Brtrue("lt");
            Emit.Ldarg_0();
            Emit.Ldarg_1();
            Emit.Cgt();
            Emit.Brtrue("gt");
            Emit.Ldc_I4_0();
            Emit.Ret();
            MarkLabel("lt");
            Emit.Ldc_I4_M1();
            Emit.Ret();
            MarkLabel("gt");
            Emit.Ldc_I4_1();
            Emit.Ret();
            throw Unreachable();
        }
    }
}
