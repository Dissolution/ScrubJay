//namespace ScrubJay.Memory.Collections;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Sequential, Size = 4)]
//#if NET8_0_OR_GREATER
//[InlineArray(4)]
//#endif
//public struct FourBytes : IBytes
//{
//    public static implicit operator FourBytes(in uint u32) =>
//        Unsafe.As<uint, FourBytes>(ref Unsafe.AsRef<uint>(in u32));
//
//    public static implicit operator FourBytes(in int i32) =>
//        Unsafe.As<int, FourBytes>(ref Unsafe.AsRef<int>(in i32));
//
//
//#if NET8_0_OR_GREATER
//    private byte _firstByte;
//#else
//    private unsafe fixed byte _bytes[4];
//#endif
//}