#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using InlineIL;
using static InlineIL.IL;
using System.Reflection;
using System.Reflection.Emit;
// ReSharper disable InvokeAsExtensionMember

namespace ScrubJay.Universal;

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
    public static bool Equals<T>(T? left, T? right, IEqualityComparer<T>? comparer)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        if (comparer is null)
            return Equals(left, right);
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
    public static bool Equals<T>(T? left, T? right,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return MethodCache<T>.LazyEquals.Value.Invoke(left, right);
    }
}

internal partial class MethodCache<T>
{
    public static readonly Lazy<Func<T?, T?, bool>> LazyEquals =
        new(CreateEqualsFunc, LazyThreadSafetyMode.ExecutionAndPublication);

    private static bool EqualsFallback(T? left, T? right)
    {
        Emit.Ldarg_0();
        Emit.Ldarg_1();
        Emit.Ceq();
        return Return<bool>();
    }

    private static Func<T?, T?, bool> CreateEqualsFunc()
    {
        Type instanceType = typeof(T);
        MethodInfo? equalsMethod = instanceType.FindMethod("Equals", typeof(bool), typeof(T));

        if (equalsMethod is null)
            return EqualsFallback;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New($"Equals_{Type.Render<T>()}", typeof(bool), typeof(T), typeof(T));
        var generator = dynamicMethod.GetILGenerator();

        // load instance
        EmitLoadInstance(generator, instanceType);
        // load value to compare to
        generator.Emit(OpCodes.Ldarg_1);
        // call the method
        generator.EmitCallMethod(instanceType, equalsMethod);
        // return the bool on the stack
        generator.Emit(OpCodes.Ret);

        if (!dynamicMethod.TryCreateDelegate<Func<T?, T?, bool>>(out var func))
        {
            func = EqualsFallback;
        }

        return func;
    }
}

#endif