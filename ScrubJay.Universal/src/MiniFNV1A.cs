using System.Security.Cryptography;

namespace ScrubJay.Universal;

#pragma warning disable CS8620, CS1573
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