using System.Buffers;

namespace ScrubJay.Collections.Pooling;

[PublicAPI]
public static class ArrayPool
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int MinCapacity<T>()
    {
        return 512 / Unsafe.SizeOf<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static int MaxCapacity<T>()
    {
        if (typeof(T) == typeof(char))
            return 0x3FFFFFDF; // string.MaxLength
        return 0X7FFFFFC7; // Array.MaxLength
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool ShouldClear<T>()
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>() || typeof(T) == typeof(char))
            return true;
        return false;
    }

    public static T[] Rent<T>()
    {
        return ArrayPool<T>.Shared.Rent(MinCapacity<T>());
    }

    public static T[] Rent<T>(int minLength)
    {
        return ArrayPool<T>.Shared.Rent(int.Clamp(minLength, MinCapacity<T>(), MaxCapacity<T>()));
    }

    public static void Return<T>(T[]? array) => Return<T>(array, ShouldClear<T>());

    public static void Return<T>(T[]? array, bool clear)
    {
        if (array is not null && array.Length > 0)
        {
            ArrayPool<T>.Shared.Return(array, clear);
        }
    }
}