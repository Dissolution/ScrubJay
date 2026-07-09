using System.Buffers;

namespace ScrubJay.Text;

[PublicAPI]
public static class TextPool
{
    private const int MIN_CAPACITY = 1024;
    private const int MAX_CAPACITY = 0x3FFFFFDF; // string.MaxLength

    private static readonly ArrayPool<char> _charArrayPool = ArrayPool<char>.Shared;

    public static char[] Rent(int minCapacity)
    {
        minCapacity = Math.Clamp(minCapacity, MIN_CAPACITY, MAX_CAPACITY);
        return _charArrayPool.Rent(minCapacity);
    }

    public static void Return(char[]? array, bool clearArray = true)
    {
        if (array is not null && array.Length > 0)
        {
            _charArrayPool.Return(array, clearArray);
        }
    }

    public static void GrowToAtLeast([AllowNull, NotNull] ref char[]? array, int newMinCapacity)
    {
        if (array is null)
        {
            array = Rent(newMinCapacity);
            return;
        }

        int len = array.Length;
        if (len >= newMinCapacity)
            return;

        var newArray = Rent(newMinCapacity);
        if (len > 0)
        {
            TextHelper.Unsafe.CopyCharacters(array, newArray, len);
            _charArrayPool.Return(array);
        }
        array = newArray;
    }
}