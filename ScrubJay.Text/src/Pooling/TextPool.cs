using System.Buffers;
using ScrubJay.Text.Utilities;

namespace ScrubJay.Text.Pooling;

[PublicAPI]
public static class TextPool
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char[] Rent(int minCapacity) 
        => ArrayPool<char>.Shared.Rent(Math.Clamp(minCapacity, 1024, string.MaxLength));

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