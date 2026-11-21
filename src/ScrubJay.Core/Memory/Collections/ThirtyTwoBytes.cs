//namespace ScrubJay.Memory.Collections;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Sequential, Size = 32)]
//#if NET8_0_OR_GREATER
//[InlineArray(32)]
//#endif
//public struct ThirtyTwoBytes : IBytes
//{
//#if NET8_0_OR_GREATER
//    private byte _firstByte;
//#else
//    private unsafe fixed byte _bytes[32];
//#endif
//}