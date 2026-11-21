//namespace ScrubJay.Memory.Collections;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Sequential, Size = 1)]
//#if NET8_0_OR_GREATER
//[InlineArray(1)]
//#endif
//public struct OneByte : IBytes
//{
//    public static implicit operator OneByte(in byte u8) =>
//        Unsafe.As<byte, OneByte>(ref Unsafe.AsRef<byte>(in u8));
//
//    public static implicit operator OneByte(in sbyte i8) =>
//        Unsafe.As<sbyte, OneByte>(ref Unsafe.AsRef<sbyte>(in i8));
//
//
//#if NET8_0_OR_GREATER
//    private byte _firstByte;
//#else
//    private unsafe fixed byte _bytes[1];
//#endif
//}