using System.Buffers;

namespace ScrubJay.Text;

[PublicAPI]
public static class TextPool
{
    internal const int MIN_CAPACITY = 512;
    internal const int MAX_CAPACITY = 0x3FFFFFDF; // string.MaxLength, which is also less than Array.MaxLength

    private static readonly ArrayPool<char> _charPool = ArrayPool<char>.Shared;

    public static char[] Rent()
        => _charPool.Rent(MIN_CAPACITY);

    public static char[] Rent(int minCapacity)
        => _charPool.Rent(int.Clamp(minCapacity, MIN_CAPACITY, MAX_CAPACITY));

    internal static char[] Rent(int literalLength, int formattedCount)
        => Rent(literalLength + (formattedCount * 16));

    public static void Return(char[]? array, bool clearArray = false)
    {
        if (array is not null && array.Length > 0)
        {
            _charPool.Return(array, clearArray);
        }
    }
}