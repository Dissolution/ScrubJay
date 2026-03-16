#pragma warning disable CS8620
// ReSharper disable MethodOverloadWithOptionalParameter

using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;

#pragma warning disable CS1573 // Parameter has no matching param tag in the XML comment (but other parameters do)

namespace ScrubJay.Universal;

internal static class MiniFNV1A
{
    private const uint FNV_PRIME = 16777619U;
    private const uint FNV_OFFSET = 2166136261U;
    private static readonly uint _startHash;

    static MiniFNV1A()
    {
#if NETFRAMEWORK || NETSTANDARD2_0
        using var rng = RandomNumberGenerator.Create();
        byte[] seed = new byte[sizeof(uint)];
        rng.GetBytes(seed);
        _startHash = (FNV_OFFSET ^ BitConverter.ToUInt32(seed, 0)) * FNV_PRIME;
#else
        Span<byte> seed = stackalloc byte[sizeof(uint)];
        RandomNumberGenerator.Fill(seed);
        _startHash = (FNV_OFFSET ^ BitConverter.ToUInt32(seed)) * FNV_PRIME;
#endif
    }

    internal static int HashBytes<T>(ref readonly T value)
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
    {
        ref T mutable = ref Unsafe.AsRef<T>(in value);
        ref byte currentByte = ref Unsafe.As<T, byte>(ref mutable);
        var length = Unsafe.SizeOf<T>();

        uint hash = _startHash;

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
        uint hash = _startHash;
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
        return MiniFNV1A.HashText(text);
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
    /// and for maximum compatibility with <see cref="object"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetHashCode<T>(ref readonly T? value,
        TypeConstraints.AllowsRefStruct<T> _ = default)
        where T : allows ref struct
    {
        if (value is null)
            return 0;
        return GetHashCodeCache<T>.Invoke(in value);
    }
    
    private static class GetHashCodeCache<T>
        where T : allows ref struct
    {
        public delegate int AnyGetHashCode(ref readonly T value);

        private static AnyGetHashCode _anyGetHashCode;
        private static bool _isTested;
        
        static GetHashCodeCache()
        {
            Type instanceType = typeof(T);
            MethodInfo? getHashCodeMethod = instanceType.FindBestMethod<AnyGetHashCode>(nameof(object.GetHashCode));

            if (getHashCodeMethod is not null)
            {
                var dynamicMethod = DynamicMethod.New<AnyGetHashCode>($"Any_{Type.Render<T>()}_GetHashCode");
                var generator = dynamicMethod.GetILGenerator();

                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Constrained, instanceType);
                generator.Emit(OpCodes.Callvirt, getHashCodeMethod);
                generator.Emit(OpCodes.Ret);

                if (dynamicMethod.TryCreateDelegate<AnyGetHashCode>(out var func))
                {
                    _anyGetHashCode = func;
                    _isTested = false;
                    return;
                }
            }

            _anyGetHashCode = (ref readonly T value) => MiniFNV1A.HashBytes<T>(in value);
            _isTested = true;
        }

        public static int Invoke(ref readonly T value)
        {
            if (_isTested)
            {
                Debug.Assert(_anyGetHashCode is not null);
                return _anyGetHashCode!(in value);
            }

            return untestedInvoke(in value);

            // Span<T> and ReadOnlySpan<T> both throw when you try to use their GetHashCode
            static int untestedInvoke(ref readonly T value)
            {
                int hashCode;
                try
                {
                    hashCode = _anyGetHashCode(in value);
                    return hashCode;
                }
                catch
                {
                    _anyGetHashCode = (ref readonly T value) => MiniFNV1A.HashBytes<T>(in value);
                    return _anyGetHashCode(in value);
                }
                finally
                {
                    _isTested = true;
                }
            }
        }
    }
}


#endif