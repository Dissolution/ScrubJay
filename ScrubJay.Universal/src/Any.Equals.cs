#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Compares two <typeparamref name="T"/> values and returns a <see cref="bool"/> indicating if they are equal.
    /// </summary>
    /// <param name="value">
    /// The first <typeparamref name="T"/> to equate.
    /// </param>
    /// <param name="other">
    ///The second <typeparamref name="T"/> to equate.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values being equated.
    /// </typeparam>
    /// <returns>
    /// <c>true</c> if <paramref name="value"/> is equal to <paramref name="other"/><br/>
    /// <c>false</c> if <paramref name="value"/> is not equal to <paramref name="other"/>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? value, T? other)
        => EqualityComparer<T>.Default.Equals(value!, other!);

    /// <summary>
    /// Compares two <typeparamref name="T"/> values with an <see cref="IEqualityComparer{T}"/> and returns a <see cref="bool"/> indicating if they are equal.
    /// </summary>
    /// <param name="value">
    /// The first <typeparamref name="T"/> to equate.
    /// </param>
    /// <param name="other">
    ///The second <typeparamref name="T"/> to equate.
    /// </param>
    /// <param name="comparer">
    /// The <see cref="IEqualityComparer{T}"/> used to determine quality between the values.<br/>
    /// If <c>null</c>, <see cref="Equals{T}(T,T)"/> will be used.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of values being equated.
    /// </typeparam>
    /// <returns>
    /// <c>true</c> if the <paramref name="comparer"/> indicated that <paramref name="value"/> is equal to <paramref name="other"/><br/>
    /// <c>false</c> if the <paramref name="comparer"/> indicated that <paramref name="value"/> is not equal to <paramref name="other"/>
    /// </returns>
    public static bool Equals<T>(T? value, T? other, IEqualityComparer<T>? comparer)
    {
        if (comparer is null)
            return Equals(value, other);
        return comparer.Equals(value!, other!);
    }

    public static bool Equals<T>(scoped ReadOnlySpan<T> left, scoped ReadOnlySpan<T> right)
#if !NET10_0_OR_GREATER
        where T : IEquatable<T>
#endif
    {
        return MemoryExtensions.SequenceEqual(left, right);
    }

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

    public static bool Equals(scoped text left, scoped text right)
    {
        return MemoryExtensions.Equals(left, right, StringComparison.Ordinal);
    }

    public static bool Equals(scoped text left, scoped text right, StringComparison comparison)
    {
        return MemoryExtensions.Equals(left, right, comparison);
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals<T>(T? value, T? other,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return MethodCache<T>.Equals(value, other);
    }

    public static bool Equals<T>(T? value, T? other,
        IEqualityComparer<T>? comparer,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (comparer is null)
            return Equals(value, other);
        return comparer.Equals(value, other);
    }
}

partial class MethodCache<T>
{
    private static readonly Lazy<Func<T?, T?, bool>> _lazyEqualsFunc =
        new(CreateEqualsFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    private static bool FallbackEquals(T? left, T? right)
    {
        if (left is null)
            return right is null;
        if (right is null)
            return false;
        if (Unsafe.AreSame<T>(in left, in right))
            return true;
        return false;
    }

    private static Func<T?, T?, bool> CreateEqualsFunc()
    {
        Type instanceType = typeof(T);
        MethodInfo? equalsMethod = instanceType.FindMethod("Equals", typeof(bool), typeof(T));

        if (equalsMethod is null)
            return FallbackEquals;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New($"{TypeName.For<T>()}_Equals", typeof(bool), typeof(T), typeof(T));
        var generator = dynamicMethod.GetILGenerator();

        // load instance
        generator.EmitLoadInstance(instanceType);
        // load value to compare to
        generator.Emit(OpCodes.Ldarg_1);
        // call the method
        generator.EmitCallMethod(instanceType, equalsMethod);
        // return the bool on the stack
        generator.Emit(OpCodes.Ret);

        if (!dynamicMethod.TryCreateDelegate<Func<T?, T?, bool>>(out var func))
        {
            func = FallbackEquals;
        }

        return func;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Equals(T? value, T? other)
    {
        return _lazyEqualsFunc.Value.Invoke(value, other);
    }
}

#endif