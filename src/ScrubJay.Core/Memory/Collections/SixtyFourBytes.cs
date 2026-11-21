//namespace ScrubJay.Memory.Collections;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Sequential, Size = 64)]
//#if NET8_0_OR_GREATER
//[InlineArray(64)]
//#endif
//public struct SixtyFourBytes : IBytes
//{
//#if NET8_0_OR_GREATER
//    private byte _firstByte;
//#else
//    private unsafe fixed byte _bytes[64];
//#endif
//}