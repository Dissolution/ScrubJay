using System.Runtime.CompilerServices;

namespace ScrubJay.Enums.SourceGen.Utilities;

/// <summary>
/// 
/// </summary>
/// <seealso href="https://datatracker.ietf.org/doc/rfc9923/"/>
/// <seealso href="https://en.wikipedia.org/w/index.php?title=Fowler%E2%80%93Noll%E2%80%93Vo_hash_function#FNV-1a_hash"/>
internal ref struct FNV1a
{
    private const uint FNV_PRIME = 0x_0100_0193U;
    private const uint FNV_OFFSET = 0x_811C_9DC5U;
    
    public static int Hash<T>(T[]? array)
    {
        if (array is null)
            return 0;
        
        unchecked
        {
            uint hash = FNV_OFFSET;
            foreach (var item in array)
            {
                if (item is not null)
                {
                    hash = hash ^ (uint)item.GetHashCode();
                }
                
                hash = hash * FNV_PRIME;
            }
            return (int)hash;
        }
    }

    public static int Hash<T1, T2>(T1? first, T2? second)
    {
        unchecked
        {
            uint hash = FNV_OFFSET;
            
            hash = hash ^ (uint)(first?.GetHashCode() ?? 0);
            hash = hash * FNV_PRIME;
        
            hash = hash ^ (uint)(second?.GetHashCode() ?? 0);
            hash = hash * FNV_PRIME;
            
            return (int)hash;
        }
    }

    private uint _hash;

    public FNV1a()
    {
        _hash = FNV_OFFSET;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Hash<T>(T? value)
    {
        if (value is not null)
        {
            return (uint)value.GetHashCode();
        }
        return 0U;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Hash<T>(T? value, IEqualityComparer<T>? comparer)
    {
        if (value is not null)
        {
            if (comparer is not null)
            {
                return (uint)comparer.GetHashCode(value);
            }
            return (uint)value.GetHashCode();
        }
        return 0U;
    }
    
    public void Add<T>(T? value)
    {
        _hash = _hash ^ Hash(value);
        _hash = _hash * FNV_PRIME;
    }
    
    public void Add<T>(T? value, IEqualityComparer<T>? comparer)
    {
        _hash = _hash ^ Hash(value, comparer);
        _hash = _hash * FNV_PRIME;
    }

    public override int GetHashCode() => (int)_hash;
}