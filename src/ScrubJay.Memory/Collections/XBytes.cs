namespace ScrubJay.Memory.Collections;

public interface IBytes;

[PublicAPI]
[StructLayout(LayoutKind.Sequential, Size = 1)]
#if NET8_0_OR_GREATER
[InlineArray(1)]
#endif
public struct OneByte : IBytes
{
    public static implicit operator OneByte(in byte u8) =>
        Unsafe.As<byte, OneByte>(ref Unsafe.AsRef<byte>(in u8));

    public static implicit operator OneByte(in sbyte i8) =>
        Unsafe.As<sbyte, OneByte>(ref Unsafe.AsRef<sbyte>(in i8));


#if NET8_0_OR_GREATER
    private byte _firstByte;
#else
    private unsafe fixed byte _bytes[1];
#endif
}

[PublicAPI]
[StructLayout(LayoutKind.Sequential, Size = 2)]
#if NET8_0_OR_GREATER
[InlineArray(2)]
#endif
public struct TwoBytes : IBytes
{
    public static implicit operator TwoBytes(in ushort u16) =>
        Unsafe.As<ushort, TwoBytes>(ref Unsafe.AsRef<ushort>(in u16));

    public static implicit operator TwoBytes(in short i16) =>
        Unsafe.As<short, TwoBytes>(ref Unsafe.AsRef<short>(in i16));

    public static implicit operator TwoBytes(scoped ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != 2)
            throw new ArgumentException("Span size must be 2", nameof(bytes));
        unsafe
        {
            fixed (byte* ptr = bytes)
            {
                return Unsafe.Read<TwoBytes>(ptr);
            }
        }
    }


#if NET8_0_OR_GREATER
    private byte _firstByte;
#else
    private unsafe fixed byte _bytes[2];
#endif
}

[PublicAPI]
[StructLayout(LayoutKind.Sequential, Size = 4)]
#if NET8_0_OR_GREATER
[InlineArray(4)]
#endif
public struct FourBytes : IBytes
{
    public static implicit operator FourBytes(in uint u32) =>
        Unsafe.As<uint, FourBytes>(ref Unsafe.AsRef<uint>(in u32));

    public static implicit operator FourBytes(in int i32) =>
        Unsafe.As<int, FourBytes>(ref Unsafe.AsRef<int>(in i32));


#if NET8_0_OR_GREATER
    private byte _firstByte;
#else
    private unsafe fixed byte _bytes[4];
#endif
}

[PublicAPI]
[StructLayout(LayoutKind.Sequential, Size = 8)]
#if NET8_0_OR_GREATER
[InlineArray(8)]
#endif
public struct EightBytes : IBytes
{
    public static implicit operator EightBytes(in ulong u64) =>
        Unsafe.As<ulong, EightBytes>(ref Unsafe.AsRef<ulong>(in u64));

    public static implicit operator EightBytes(in long i64) =>
        Unsafe.As<long, EightBytes>(ref Unsafe.AsRef<long>(in i64));

#if NET8_0_OR_GREATER
    private byte _firstByte;
#else
    private unsafe fixed byte _bytes[8];
#endif
}

[PublicAPI]
[StructLayout(LayoutKind.Sequential, Size = 16)]
#if NET8_0_OR_GREATER
[InlineArray(16)]
#endif
public struct SixteenBytes : IBytes
{
#if NET7_0_OR_GREATER
    public static implicit operator SixteenBytes(in UInt128 u128) =>
        Unsafe.As<UInt128, SixteenBytes>(ref Unsafe.AsRef<UInt128>(in u128));

    public static implicit operator SixteenBytes(in Int128 i128) =>
        Unsafe.As<Int128, SixteenBytes>(ref Unsafe.AsRef<Int128>(in i128));
#endif

#if NET8_0_OR_GREATER
    private byte _firstByte;
#else
    private unsafe fixed byte _bytes[16];
#endif
}

[PublicAPI]
[StructLayout(LayoutKind.Sequential, Size = 32)]
#if NET8_0_OR_GREATER
[InlineArray(32)]
#endif
public struct ThirtyTwoBytes : IBytes
{
#if NET8_0_OR_GREATER
    private byte _firstByte;
#else
    private unsafe fixed byte _bytes[32];
#endif
}

[PublicAPI]
[StructLayout(LayoutKind.Sequential, Size = 64)]
#if NET8_0_OR_GREATER
[InlineArray(64)]
#endif
public struct SixtyFourBytes : IBytes
{
#if NET8_0_OR_GREATER
    private byte _firstByte;
#else
    private unsafe fixed byte _bytes[64];
#endif
}