//using System.Buffers.Binary;
//using InlineIL;
//
//namespace ScrubJay.Memory;
//
///// <summary>
///// 
///// </summary>
///// <seealso cref="BitConverter"/>
///// <seealso cref="BinaryPrimitives"/>
//[PublicAPI]
//public static class Bits
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static T Clone<T>(ref readonly T value)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        unsafe
//        {
//            if (sizeof(T) == 1)
//            {
//                Emit.Ldarg(nameof(value));
//                Emit.Unaligned(0x1);
//                Emit.Ldind_U1();
//                return Return<T>();
//            }
//            if (sizeof(T) == 2)
//            {
//                Emit.Ldarg(nameof(value));
//                Emit.Unaligned(0x1);
//                Emit.Ldind_U2();
//                return Return<T>();
//            }
//            if (sizeof(T) == 4)
//            {
//                Emit.Ldarg(nameof(value));
//                Emit.Unaligned(0x1);
//                Emit.Ldind_U4();
//                return Return<T>();
//            }
//            if (sizeof(T) == 8)
//            {
//                Emit.Ldarg(nameof(value));
//                Emit.Unaligned(0x1);
//                Emit.Ldind_I8();
//                return Return<T>();
//            }
//
//            Emit.Ldarg(nameof(value));
//            Emit.Unaligned(0x1);
//            Emit.Ldobj<T>();
//            return Return<T>();
//        }
//    }
//
//    public static ref readonly TOut Reinterpret<TIn, TOut>(ref readonly TIn input)
//#if NET9_0_OR_GREATER
//        where TIn : unmanaged, allows ref struct
//        where TOut : unmanaged, allows ref struct
//#else
//        where TIn : unmanaged
//        where TOut : unmanaged
//#endif
//    {
//        Emit.Ldarg(nameof(input));
//        Emit.Ret();
//        throw Unreachable();
//    }
//
//    public static ref TOut ReinterpretRef<TIn, TOut>(ref TIn input)
//#if NET9_0_OR_GREATER
//        where TIn : unmanaged, allows ref struct
//        where TOut : unmanaged, allows ref struct
//#else
//        where TIn : unmanaged
//        where TOut : unmanaged
//#endif
//    {
//        Emit.Ldarg(nameof(input));
//        Emit.Ret();
//        throw Unreachable();
//    }
//
//
//#region Reversed (T -> T)
//    public static T Reversed<T>(ref readonly T value)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        var clone = Clone(in value);
//        AsBytes(ref clone).Reverse();
//        return clone;
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static byte Reversed(byte value) => value;
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static sbyte Reversed(sbyte value) => value;
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static ushort Reversed(ushort value)
//    {
//        return (ushort)((value >> 8) + (value << 8));
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static short Reversed(short value)
//    {
//        return (short)((value >> 8) + (value << 8));
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static uint Reversed(uint value)
//    {
//        return BitOperations.RotateRight(value & 0x00FF00FFu, 8)
//            + BitOperations.RotateLeft(value & 0xFF00FF00u, 8);
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static int Reversed(int value) => (int)Reversed((uint)value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static ulong Reversed(ulong value)
//    {
//        return ((ulong)Reversed((uint)value) << 32)
//            + Reversed((uint)(value >> 32));
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static long Reversed(long value) => (long)Reversed((ulong)value);
//
//#if NET8_0_OR_GREATER
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static UInt128 Reversed(UInt128 value) => BinaryPrimitives.ReverseEndianness(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Int128 Reversed(Int128 value) => BinaryPrimitives.ReverseEndianness(value);
//    
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static nuint Reversed(nuint value) => BinaryPrimitives.ReverseEndianness(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static nint Reversed(nint value) => BinaryPrimitives.ReverseEndianness(value);
//#else
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static nuint Reversed(nuint value)
//    {
//        if (Unsafe.SizeOf<nuint>() == sizeof(ulong))
//            return (nuint)Reversed((ulong)value);
//        return (nuint)Reversed((uint)value);
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static nint Reversed(nint value)
//    {
//        if (Unsafe.SizeOf<nint>() == sizeof(long))
//            return (nint)Reversed((long)value);
//        return (nint)Reversed((int)value);
//    }
//#endif
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static char Reversed(char value) => (char)Reversed((ushort)value);
//#endregion /Reversed
//
//#region Reverse (ref T)
//    public static void Reverse<T>(ref T value)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        AsBytes(ref value).Reverse();
//    }
//#endregion /Reverse
//
//    public static readonly Endianness SystemEndianness = BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;
//
//#region Convert / As (bytes <-> T)
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static ReadOnlySpan<byte> AsReadOnlyBytes<T>(ref readonly T value)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        Emit.Ldarg(nameof(value)); // treat as void*
//        Emit.Sizeof<T>();
//        Emit.Newobj(MethodRef.Constructor(typeof(ReadOnlySpan<byte>), [typeof(void*), typeof(int)]));
//        Emit.Ret();
//        throw Unreachable();
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Span<byte> AsBytes<T>(ref T value)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        Emit.Ldarg(nameof(value)); // treat as void*
//        Emit.Sizeof<T>();
//        Emit.Newobj(MethodRef.Constructor(typeof(Span<byte>), [typeof(void*), typeof(int)]));
//        Emit.Ret();
//        throw Unreachable();
//    }
//#endregion /Convert
//
//#region (Try) Writing (T into bytes)
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static void UnsafeWrite<T>(ref readonly T value, scoped Span<byte> destination)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        Emit.Ldarg(nameof(destination));
//        Emit.Ldarg(nameof(value));
//        Emit.Sizeof<T>();
//        Emit.Unaligned(0x01);
//        Emit.Cpblk();
//        Emit.Ret();
//    }
//
//    public static Option<int> TryWrite<T>(ref readonly T value, Span<byte> destination)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (destination.Length < Unsafe.SizeOf<T>())
//            return None;
//        UnsafeWrite<T>(in value, destination);
//        return Some(Unsafe.SizeOf<T>());
//    }
//
//    public static Option<int> TryWrite<T>(ref readonly T value, Span<byte> destination, Endianness endianness)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        if (destination.Length < Unsafe.SizeOf<T>())
//            return None;
//
//        if (endianness.IsNonSystem)
//        {
//            var esrever = Reversed(in value);
//            UnsafeWrite(in esrever, destination);
//        }
//        else
//        {
//            UnsafeWrite<T>(in value, destination);
//        }
//
//        return Some(Unsafe.SizeOf<T>());
//    }
//#endregion /Writing
//
//
//#region (Try) Reading (bytes -> T)
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static T UnsafeRead<T>(scoped ReadOnlySpan<byte> bytes)
//        where T : unmanaged
//    {
//        Emit.Ldarg(nameof(bytes)); // treat span<byte> as byte*
//        Emit.Unaligned(0x1);
//        Emit.Ldobj<T>();
//        return Return<T>();
//    }
//
//
//    public static Option<T> TryRead<T>(scoped ReadOnlySpan<byte> source)
//        where T : unmanaged
//    {
//        if (source.Length < Unsafe.SizeOf<T>())
//            return None;
//        T value = UnsafeRead<T>(source);
//        return Some<T>(value);
//    }
//
//    public static Option<T> TryRead<T>(scoped ReadOnlySpan<byte> source, Endianness endianness)
//        where T : unmanaged
//    {
//        if (source.Length < Unsafe.SizeOf<T>())
//            return None;
//        T value = UnsafeRead<T>(source);
//        if (endianness.IsNonSystem)
//            Reverse(ref value);
//        return Some<T>(value);
//    }
//#endregion
//
//
//#region Bit Casting
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static TTo BitCast<TFrom, TTo>(TFrom source)
//#if NET9_0_OR_GREATER
//        where TFrom : allows ref struct
//        where TTo : allows ref struct
//#endif
//    {
//        Emit.Ldarg(nameof(source));
//        Emit.Unaligned(0x1);
//        Emit.Ldobj<TTo>();
//        return Return<TTo>();
//    }
//
//    public static bool TryBitCast<TIn, TOut>(TIn input, out TOut output)
//        where TIn : unmanaged
//        where TOut : unmanaged
//    {
//        if (Unsafe.SizeOf<TIn>() != Unsafe.SizeOf<TOut>())
//        {
//            output = default;
//            return false;
//        }
//
//        Emit.Ldarg(nameof(output));
//        Emit.Ldarg(nameof(input));
//        Emit.Unaligned(0x1);
//        Emit.Stobj<TOut>();
//        Emit.Ldc_I4_1();
//        Emit.Ret();
//        throw Unreachable();
//    }
//
//#if NET6_0_OR_GREATER
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Half UInt16ToF16(ushort value) => BitCast<ushort, Half>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static ushort F16ToUInt16(Half value) => BitCast<Half, ushort>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static Half Int16ToF16(short value) => BitCast<short, Half>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static short F16ToInt16Bits(Half value) => BitCast<Half, short>(value);
//#endif
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static float UInt32ToF32(uint value) => BitCast<uint, float>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static uint F32ToUInt32(float value) => BitCast<float, uint>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static float Int32ToF32(int value) => BitCast<int, float>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static int F32ToInt32(float value) => BitCast<float, int>(value);
//
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static double Int64ToF64(long value) => BitCast<long, double>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static long F64ToInt64(double value) => BitCast<double, long>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static double UInt64ToF64(ulong value) => BitCast<ulong, double>(value);
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static ulong F64ToUInt64(double value) => BitCast<double, ulong>(value);
//#endregion
//}