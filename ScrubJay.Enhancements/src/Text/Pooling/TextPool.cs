using System.Buffers;
using ScrubJay.Enhancements.Text.Utilities;

namespace ScrubJay.Enhancements.Text.Pooling;

[PublicAPI]
public static class TextPool
{
    public const int MinLength = 1024;
    public const int MaxLength = 0x3FFFFFDF; // = string.MaxLength

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char[] Rent(int minCapacity)
        => ArrayPool<char>.Shared.Rent(Math.Clamp(minCapacity, MinLength, MaxLength));

    public static void Return(char[]? characters, bool clean = false)
    {
        if (characters is not null)
        {
            if (clean)
            {
                TextHelper.Clear(characters);
            }
            ArrayPool<char>.Shared.Return(characters, false);
        }
    }
}