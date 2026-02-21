#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace ScrubJay.Universal;

partial class Any
{
    /// <summary>
    /// Gets a hashcode for a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to hash, may be <c>null</c>.
    /// </param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(T? value)
    {
        if (value is null)
            return 0;

        return value.GetHashCode();
    }

    /// <summary>
    /// Gets a hashcode for some <see cref="text"/>.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode(scoped text text)
    {
#if NETSTANDARD2_0 || NETFRAMEWORK
        // FNV-1a (https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function#FNV-1a_hash)
        // 32-bit
        unchecked
        {
            const uint FNV_PRIME = 16777619;
            uint hash = 2166136261; // FNV_OFFSET

            for (int i = 0; i < text.Length; i++)
            {
                hash ^= text[i];
                hash *= FNV_PRIME;
            }

            return (int)hash;
        }
#elif NETSTANDARD2_1
        HashCode hasher = new();
        foreach (char ch in text)
        {
            hasher.Add<char>(ch);
        }
        return hasher.ToHashCode();
#else
        return string.GetHashCode(
            text,
            StringComparison.Ordinal);
#endif
    }
}

#if NET9_0_OR_GREATER
partial class Any
{
    /// <summary>
    /// Gets a hashcode for a <typeparamref name="T"/> <paramref name="value"/>.
    /// </summary>
    /// <param name="value">
    /// The <typeparamref name="T"/> value to hash, may be <c>null</c>.
    /// </param>
    /// <typeparam name="T">
    /// The <see cref="Type"/> of <paramref name="value"/> being hashed, <i>may</i> be a <c>ref struct</c>.
    /// </typeparam>
    /// <returns>
    /// An <see cref="int"/> hashcode of the <typeparamref name="T"/> <paramref name="value"/>.
    /// </returns>
    /// <remarks>
    /// This method may not have to exist, as <c>ref struct</c> values cannot currently be stored in a HashSet nor Dictionary.
    /// But it does to allow for the possibility that more stack-based collections could exist in the future,
    /// and for maximum compatability with <see cref="object"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(T? value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        return MethodCache<T>.GetHashCode(value);
    }
}

partial class MethodCache<T>
{
    private static readonly Lazy<Func<T, int>?> _lazyGetHashCodeFunc = new(
        CreateGetHashCodeFunc,
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static Func<T, int>? CreateGetHashCodeFunc()
    {
        Type instanceType = typeof(T);

        MethodInfo? getHashCodeMethod = instanceType.FindMethod(
            "GetHashCode",
            typeof(int));

        if (getHashCodeMethod is null)
            return null;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New(
            $"{TypeName.For<T>()}_GetHashCode",
            typeof(int),
            typeof(T));

        var generator = dynamicMethod.GetILGenerator();

        // load instance
        generator.EmitLoadInstance(instanceType);

        // call the method
        generator.EmitCallMethod(
            instanceType,
            getHashCodeMethod);

        // return the int on the stack
        generator.Emit(OpCodes.Ret);

        if (!dynamicMethod.TryCreateDelegate<Func<T, int>>(out var func))
            return null;

        // try to execute it to see if it will even work
        // Span + ReadOnlySpan throw
        try
        {
            _ = func(default!);
        }
#pragma warning disable
        catch
#pragma warning restor
        {
            return null;
        }

        return func;
    }

    public static int GetHashCode(T? value)
    {
        if (value is not null)
        {
            var func = _lazyGetHashCodeFunc.Value;

            if (func is not null)
            {
                return func(value);
            }
        }

        return 0;
    }
}

#endif