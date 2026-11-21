//namespace ScrubJay.Memory.Collections;
//
//[PublicAPI]
//[StructLayout(LayoutKind.Sequential, Size = 2)]
//#if NET8_0_OR_GREATER
//[InlineArray(2)]
//#endif
//public struct TwoBytes : IBytes
//{
//    public static implicit operator TwoBytes(in ushort u16) =>
//        Unsafe.As<ushort, TwoBytes>(ref Unsafe.AsRef<ushort>(in u16));
//
//    public static implicit operator TwoBytes(in short i16) =>
//        Unsafe.As<short, TwoBytes>(ref Unsafe.AsRef<short>(in i16));
//
//    public static implicit operator TwoBytes(scoped ReadOnlySpan<byte> bytes)
//    {
//        if (bytes.Length != 2)
//            throw new ArgumentException("Span size must be 2", nameof(bytes));
//        unsafe
//        {
//            fixed (byte* ptr = bytes)
//            {
//                return Unsafe.Read<TwoBytes>(ptr);
//            }
//        }
//    }
//
//
//#if NET8_0_OR_GREATER
//    private byte _firstByte;
//#else
//    private unsafe fixed byte _bytes[2];
//#endif
//}