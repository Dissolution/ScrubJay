using static InlineIL.IL;

namespace ScrubJay.Enhancements.Extensions;

public static class CharExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void* AsVoidPointer(ref readonly char ch)
    {
        Emit.Ldarg_0();
        return ReturnPointer();
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<char> AsSpan(this ref readonly char ch)
    {
#if NET7_0_OR_GREATER
        return new(in ch);
#else
        unsafe
        {
            return new(AsVoidPointer(in ch), 1);
        }
#endif
    }
}