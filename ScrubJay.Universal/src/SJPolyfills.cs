#if NETFRAMEWORK || NETSTANDARD2_0

namespace ScrubJay.Universal
{
    [PublicAPI]
    public static class PolyfillExtensions
    {
        extension(Type? type)
        {
            public bool IsByRefLike => false;
        }
    }
}
#endif

#if NETFRAMEWORK || NETSTANDARD
namespace System.Numerics
{
    [PublicAPI]
    public static class BitOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [CLSCompliant(false)]
        public static uint RotateLeft(uint value, int offset)
            => (value << offset) | (value >> (32 - offset));
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [CLSCompliant(false)]
        public static uint RotateRight(uint value, int offset)
            => (value >> offset) | (value << (32 - offset));
    }
}

#endif