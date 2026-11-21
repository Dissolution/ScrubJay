using System.Buffers;

namespace ScrubJay.Text;

[PublicAPI]
public static class TextPool
{
    internal const int MIN_CAPACITY = 512;
    internal const int MAX_CAPACITY = 0x3FFFFFDF; // string.MaxLength

    private static readonly ArrayPool<char> _characterPool = ArrayPool<char>.Shared;

    public static char[] Rent() => _characterPool.Rent(MIN_CAPACITY);

    public static char[] Rent(int minCapacity)
    {
        return _characterPool.Rent(int.Clamp(minCapacity, MIN_CAPACITY, MAX_CAPACITY));
    }

    public static char[] Rent(int literalLength, int formattedCount)
    {
        return _characterPool.Rent(int.Clamp(literalLength + (formattedCount * 16), MIN_CAPACITY, MAX_CAPACITY));
    }

    public static void Return(char[]? array, bool clearArray = true)
    {
        if (array is not null && array.Length > 0)
        {
            _characterPool.Return(array, clearArray);
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
            TextHelper.Notsafe.CopyCharacters(array, newArray, len);
            _characterPool.Return(array);
        }
        array = newArray;
    }
}