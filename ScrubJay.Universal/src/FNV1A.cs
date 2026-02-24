namespace ScrubJay.Universal;

/// <summary>
/// A simple FNV-1a 32-bit deterministic hash generator
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function#FNV-1a_hash"/>
[PublicAPI]
internal struct FNV1aHasher
{
    const uint FNV_PRIME = 16777619U;
    const uint FNV_OFFSET = 2166136261U;

#if NET9_0_OR_GREATER
    internal static int UnsafeHashBits<T>(ref readonly T value)
        where T : allows ref struct
    {
        var span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, byte>(ref Unsafe.AsRef<T>(in value)), Unsafe.SizeOf<T>());

        uint hash = FNV_OFFSET;

        unchecked
        {
            for (int i = 0; i < span.Length; i++)
            {
                hash ^= span[i];
                hash *= FNV_PRIME;
            }

            return (int)hash;
        }
    }
#endif
    
    public static int HashCharacters(scoped ReadOnlySpan<char> text)
    {
        unchecked
        {
            uint hash = FNV_OFFSET;

            for (int i = 0; i < text.Length; i++)
            {
                hash ^= text[i];
                hash *= FNV_PRIME;
            }

            return (int)hash;
        }
    }
    
    

    private uint _hash = FNV_OFFSET;

    public FNV1aHasher()
    {

    }

    public void Add(char ch)
    {
        unchecked
        {
            _hash ^= ch;
            _hash *= FNV_PRIME;
        }
    }

   

    public int GetI32Hash() => unchecked((int)_hash);

    public uint GetU32Hash() => _hash;
}