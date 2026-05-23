namespace ScrubJay.Text.Memory;

internal static class SystemExtensions
{
#if NET8_0_OR_GREATER
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lower")]
    private static extern ref ulong RefLower(UInt128 u128);
    
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_upper")]
    private static extern ref ulong RefUpper(UInt128 u128);

    extension(UInt128 u128)
    {
        public ulong Lower => RefLower(u128);
        
        public ulong Upper => RefUpper(u128);
    }




#endif
}