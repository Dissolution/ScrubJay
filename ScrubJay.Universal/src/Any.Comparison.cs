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
    /// <param name="value">
    /// The first <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="other">
    /// The second <typeparamref name="T"/> value to compare.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to compare.
    /// </typeparam>
    /// <returns>
    /// <c>&lt;0</c> if <paramref name="value"/> is less than <paramref name="other"/><br/>
    /// <c>0</c> if <paramref name="value"/> is equal to <paramref name="other"/><br/>
    /// <c>&gt;0</c> if <paramref name="value"/> is greater than <paramref name="other"/>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare<T>(T? value, T? other) => Comparer<T>.Default.Compare(value!, other!);

    /// <summary>
    /// Compares two <typeparamref name="T"/> values with an <see cref="IComparer{T}"/> and returns an <see cref="int"/> indicating their relation.
    /// </summary>
    /// <param name="value">
    /// The first <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="other">
    /// The second <typeparamref name="T"/> value to compare.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IComparer{T}"/> that determines the relation between <paramref name="value"/> and <paramref name="other"/>.<br/>
    /// If <c>null</c>, <see cref="Compare{T}(T,T)"/> will be used.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values to compare.
    /// </typeparam>
    /// <returns>
    /// <c>&lt;0</c> if <paramref name="comparer"/> indicates that <paramref name="value"/> is less than <paramref name="other"/><br/>
    /// <c>0</c> if <paramref name="comparer"/> indicates that<paramref name="value"/> is equal to <paramref name="other"/><br/>
    /// <c>&gt;0</c> if <paramref name="comparer"/> indicates that<paramref name="value"/> is greater than <paramref name="other"/>
    /// </returns>
    public static int Compare<T>(T? value, T? other, IComparer<T>? comparer)
    {
        if (comparer is null)
            return Compare(value, other);
        return comparer.Compare(value!, other!);
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
    public static int Compare<T>(T? value, T? other,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return MethodCache<T>.Compare(value, other);
    }

    public static int Compare<T>(T? value, T? other,
        IComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (comparer is null)
            return Compare(value, other);
        return comparer.Compare(value, other);
    }
}

partial class MethodCache<T>
{
    private static readonly Lazy<Func<T?, T?, int>> _lazyCompareFunc =
        new(CreateCompareFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    private static int FallbackCompare(T? left, T? right)
    {
        if (left is null)
        {
            if (right is null)
            {
                return 0; // nulls are the same
            }
            else
            {
                return -1; // null is the 'smallest' value
            }
        }
        else
        {
            if (right is null)
            {
                return 1; // everything is 'bigger' than null
            }
            else
            {
                return 0; // we have no other way to compare these values
            }
        }
    }

    private static Func<T?, T?, int> CreateCompareFunc()
    {
        Type instanceType = typeof(T);
        MethodInfo? compareToMethod = instanceType.FindMethod("CompareTo", typeof(int), typeof(T));

        if (compareToMethod is null)
            return FallbackCompare;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New($"{TypeName.For<T>()}_Compare", typeof(int), typeof(T), typeof(T));
        var generator = dynamicMethod.GetILGenerator();

        // load instance
        generator.EmitLoadInstance(instanceType);
        // load value to compare to
        generator.Emit(OpCodes.Ldarg_1);
        // call the method
        generator.EmitCallMethod(instanceType, compareToMethod);
        // return the int on the stack
        generator.Emit(OpCodes.Ret);

        if (!dynamicMethod.TryCreateDelegate<Func<T?, T?, int>>(out var func))
        {
            func = FallbackCompare;
        }

        return func;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Compare(T? value, T? other)
    {
        return _lazyCompareFunc.Value.Invoke(value, other);
    }
}

#endif