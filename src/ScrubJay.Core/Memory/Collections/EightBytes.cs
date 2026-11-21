//namespace ScrubJay.Memory.Collections;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Sequential, Size = 8)]
//#if NET8_0_OR_GREATER
//[InlineArray(8)]
//#endif
//public struct EightBytes : IBytes
//{
//    public static implicit operator EightBytes(in ulong u64) =>
//        Unsafe.As<ulong, EightBytes>(ref Unsafe.AsRef<ulong>(in u64));
//
//    public static implicit operator EightBytes(in long i64) =>
//        Unsafe.As<long, EightBytes>(ref Unsafe.AsRef<long>(in i64));
//
//#if NET8_0_OR_GREATER
//    private byte _firstByte;
//#else
//    private unsafe fixed byte _bytes[8];
//#endif
//}