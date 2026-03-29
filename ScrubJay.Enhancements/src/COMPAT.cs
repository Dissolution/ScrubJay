#pragma warning disable all

#if NETSTANDARD || NETFRAMEWORK
namespace System.Numerics
{
    public static class BitOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint RotateLeft(uint value, int offset) => (value << offset) | (value >> (32 - offset));
    }
}
#endif