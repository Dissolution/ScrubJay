namespace ScrubJay.Text.Pooling;

/// <summary>
/// A utility class that manages the pooling of <see cref="char"/><see cref="Array">[]</see> instances.
/// </summary>
[PublicAPI]
public static class TextPool
{
    public const int MinLength = 1024;
    public const int MaxLength = 0x3FFFFFDF; // = string.MaxLength

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ClampCapacity(int capacity)
    {
        if (capacity < MinLength)
            return MinLength;
        if (capacity > MaxLength)
            return MaxLength;
        return capacity;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char[] Rent(int minCapacity)
        => ArrayPool<char>.Shared.Rent(ClampCapacity(minCapacity));

    public static void Return(char[]? characters, bool clean = false)
    {
        if (characters is not null && characters.Length > 0)
        {
            if (clean)
            {
                TextHelper.Clear(characters);
            }
            ArrayPool<char>.Shared.Return(characters, false);
        }
    }

    public static void GrowBy([AllowNull, JetBrains.Annotations.NotNull] ref char[]? array, int count)
    {
        if (count > 0)
        {
            if (array is not null)
            {
                int len = array.Length;
                var newArray = Rent(len + count);
                TextHelper.Unsafe.CopyTo(array, newArray, len);
                var toReturn = Interlocked.Exchange<char[]>(ref array, newArray);
                Return(toReturn);
            }
            else
            {
                array = Rent(count);
            }
        }
        else
        {
            array = [];
        }
    }
}