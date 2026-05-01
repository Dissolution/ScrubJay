using SystemUnsafe = System.Runtime.CompilerServices.Unsafe;

namespace ScrubJay.Text.Utilities;

[PublicAPI]
public static partial class TextHelper
{
    public static bool TryUnboxText(object? obj, out ReadOnlySpan<char> text)
    {
        if (obj is char)
        {
#if NET7_0_OR_GREATER
            text = new(ref SystemUnsafe.Unbox<char>(obj));
#else
            ref char ch = ref SystemUnsafe.Unbox<char>(obj);
            text = ch.AsSpan();
#endif
            return true;
        }

        if (obj is string str)
        {
            text = str.AsSpan();
            return true;
        }

        if (obj is char[] chars)
        {
            text = chars.AsSpan();
            return true;
        }

        if (obj is ReadOnlyMemory<char> memory)
        {
            text = memory.Span;
            return true;
        }

        text = default;
        return false;
    }
}