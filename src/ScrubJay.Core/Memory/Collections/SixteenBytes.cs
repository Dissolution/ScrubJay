//namespace ScrubJay.Memory.Collections;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Sequential, Size = 16)]
//#if NET8_0_OR_GREATER
//[InlineArray(16)]
//#endif
//public struct SixteenBytes : IBytes
//{
//#if NET7_0_OR_GREATER
//    public static implicit operator SixteenBytes(in UInt128 u128) =>
//        Unsafe.As<UInt128, SixteenBytes>(ref Unsafe.AsRef<UInt128>(in u128));
//
//    public static implicit operator SixteenBytes(in Int128 i128) =>
//        Unsafe.As<Int128, SixteenBytes>(ref Unsafe.AsRef<Int128>(in i128));
//#endif
//
//#if NET8_0_OR_GREATER
//    private byte _firstByte;
//#else
//    private unsafe fixed byte _bytes[16];
//#endif
//}