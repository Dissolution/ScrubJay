#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Compares two <typeparamref name="T"/> values and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="left">
    /// The first <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="right">
    /// The second <typeparamref name="T"/> value to compare.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to compare.
    /// </typeparam>
    /// <returns>
    /// <c>&lt;0</c> if <paramref name="left"/> is less than <paramref name="right"/><br/>
    /// <c>0</c> if <paramref name="left"/> is equal to <paramref name="right"/><br/>
    /// <c>&gt;0</c> if <paramref name="left"/> is greater than <paramref name="right"/>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(in T? left, in T? right) => Comparer<T>.Default.Compare(left!, right!);

    /// <summary>
    /// Compares two <typeparamref name="T"/> values with an <see cref="IComparer{T}"/> and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="left">
    /// The first <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="right">
    /// The second <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> that determines the relation between <paramref name="left"/> and <paramref name="right"/>.<br/>
    /// If <c>null</c>, <see cref="Compare{T}(T,T)"/> will be used.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to compare.
    /// </typeparam>
    /// <returns>
    /// <c>&lt;0</c> if <paramref name="comparer"/> indicates that <paramref name="left"/> is less than <paramref name="right"/><br/>
    /// <c>0</c> if <paramref name="comparer"/> indicates that<paramref name="left"/> is equal to <paramref name="right"/><br/>
    /// <c>&gt;0</c> if <paramref name="comparer"/> indicates that<paramref name="left"/> is greater than <paramref name="right"/>
    /// </returns>
    public static int Compare<T>(in T? left, in T? right, IComparer<T>? comparer)
    {
        if (comparer is null)
            return Compare<T>(left, right);
        return comparer.Compare(left!, right!);
    }

    /// <summary>
    /// Compares two <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;T&gt;s</see> and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(scoped ReadOnlySpan<T> left, scoped ReadOnlySpan<T> right)
#if !NET10_0_OR_GREATER
        where T : IComparable<T>
#endif
    {
        return MemoryExtensions.SequenceCompareTo(left, right);
    }

    /// <summary>
    /// Compares two <see cref="ReadOnlySpan{T}">ReadOnlySpan&lt;T&gt;s</see> with an <see cref="IComparer{T}"/> and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static int Compare<T>(
        scoped ReadOnlySpan<T> left,
        scoped ReadOnlySpan<T> right,
        IComparer<T>? comparer)
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

    /// <summary>
    /// Compares two <see cref="text"/> values and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <c>&lt;0</c> if <paramref name="left"/> is less than <paramref name="right"/><br/>
    /// <c>0</c> if <paramref name="left"/> is equal to <paramref name="right"/><br/>
    /// <c>&gt;0</c> if <paramref name="left"/> is greater than <paramref name="right"/>
    /// </returns>
    public static int Compare(scoped text left, scoped text right)
    {
        return MemoryExtensions.CompareTo(left, right, StringComparison.Ordinal);
    }

    /// <summary>
    /// Compares two <see cref="text"/> values with a <see cref="StringComparison"/> and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="comparison"></param>
    /// <returns></returns>
    public static int Compare(scoped text left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.CompareTo(left, right, comparison);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
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
        where T : allows ref struct
    {
        return CompareCache<T>.Invoke(in left, in right);
    }
    
    private static class CompareCache<T>
        where T : allows ref struct
    {
        public delegate int AnyCompare(ref readonly T? left, ref readonly T? right);

        public static readonly AnyCompare Invoke;
        
        static CompareCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? compareToMethod = instanceType.FindBestMethod("CompareTo", typeof(int), typeof(T));

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
#endif