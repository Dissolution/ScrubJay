#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace ScrubJay.Universal;

internal static class MiniFNV1a
{
    private const uint FNV_PRIME = 16777619U;
    private const uint FNV_OFFSET = 2166136261U;

    internal static int HashBytes<T>(ref readonly T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        ref T mutable = ref Unsafe.AsRef(in value);
        ref byte currentByte = ref Unsafe.As<T, byte>(ref mutable);
        var length = Unsafe.SizeOf<T>();

        uint hash = FNV_OFFSET;

        unchecked
        {
            // 4-byte chunks
            while (length >= 4)
            {
                uint slice = Unsafe.ReadUnaligned<uint>(ref currentByte);

                hash ^= slice;
                hash *= FNV_PRIME;

                currentByte = ref Unsafe.Add(ref currentByte, 4);
                length -= 4;
            }

            // remaining bytes
            while (length > 0)
            {
                hash ^= currentByte;
                hash *= FNV_PRIME;

                currentByte = ref Unsafe.Add(ref currentByte, 1);
                length--;
            }

            return (int)hash;
        }
    }

    internal static int HashText(scoped ReadOnlySpan<char> text)
    {
        uint hash = FNV_OFFSET;
        unchecked
        {
            foreach (var ch in text)
            {
                hash ^= ch;
                hash *= FNV_PRIME;
            }
            return (int)hash;
        }
    }
}

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
    public static int GetHashCode<T>(ref readonly T? value)
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
    public static int GetHashCode(ref readonly text text)
    {
#if NETSTANDARD2_0 || NETFRAMEWORK
        // simple FNV1a
        return MiniFNV1a.HashText(text);
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
    public static int GetHashCode<T>(ref readonly T? value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return 0;
        return MethodCache<T>.LazyGetHashCode.Value.Invoke(in value);
    }
}

partial class MethodCache<T>
{
    public delegate int AnyGetHashCode(ref readonly T value);
    
    public static readonly Lazy<AnyGetHashCode> LazyGetHashCode = new(
        CreateGetHashCodeFunc,
        LazyThreadSafetyMode.ExecutionAndPublication);

    private static int FallbackGetHashCode(ref readonly T value)
    {
        return MiniFNV1a.HashBytes<T>(in value);
    }

    private static AnyGetHashCode CreateGetHashCodeFunc()
    {
        Type instanceType = typeof(T);
        MethodInfo? getHashCodeMethod = FindBestMethod<AnyGetHashCode>(instanceType, nameof(object.GetHashCode));

        if (getHashCodeMethod is null)
            return FallbackGetHashCode;

        // emit our dynamic method
        var dynamicMethod = DynamicMethod.New<AnyGetHashCode>($"Any_{Type.Render<T>()}_GetHashCode");
        var generator = dynamicMethod.GetILGenerator();
        
        generator.Emit(OpCodes.Ldarg_0);
        generator.Emit(OpCodes.Constrained, instanceType);
        generator.Emit(OpCodes.Callvirt, getHashCodeMethod);
        generator.Emit(OpCodes.Ret);
        
        if (!dynamicMethod.TryCreateDelegate<AnyGetHashCode>(out var func))
        {
            func = FallbackGetHashCode;
        }

        return func;
    }
}

#endif