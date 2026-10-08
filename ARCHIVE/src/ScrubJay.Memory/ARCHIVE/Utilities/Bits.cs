//#if NET8_0_OR_GREATER
//using System.Buffers.Binary;
//#endif
//
//namespace ScrubJay.Memory.ARCHIVE.Utilities;
//
//[PublicAPI]
//public static class Bits
//{
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    public static ReadOnlySpan<byte> AsReadOnlyBytes<T>(ref readonly T value)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
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
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        Emit.Ldarg(nameof(value)); // treat as void*
//        Emit.Sizeof<T>();
//        Emit.Newobj(MethodRef.Constructor(typeof(Span<byte>), [typeof(void*), typeof(int)]));
//        Emit.Ret();
//        throw Unreachable();
//    }
//    
//    
//    
//#region Reversed (T -> T)
//    extension(Unmanaged)
//    {
//        public static T Reversed<T>(ref readonly T value)
//            where T : unmanaged
//#if NET9_0_OR_GREATER
//            , allows ref struct
//#endif
//        {
//            var clone = Unmanaged.Clone(in value);
//            Unmanaged.AsBytes(ref clone).Reverse();
//            return clone;
//        }
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
//    public static char Reversed(char value) => (char)Reversed((ushort)value);
//#endregion Reversed
//
//#region Reverse (ref T)
//
//    public static void Reverse<T>(ref T value)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        AsBytes(ref value).Reverse();
//    }
//    
//#endregion /Reverse
//
//    public static readonly Endianness SystemEndianness = BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;
//
//#region Writing
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static void UnsafeWriteTo<T>(ref readonly T value, ref byte destination)
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
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static void UnsafeWriteTo<T>(ref readonly T value, scoped Span<byte> destination)
//        where T : unmanaged
//#if NET9_0_OR_GREATER
//        , allows ref struct
//#endif
//    {
//        UnsafeWriteTo<T>(in value, ref destination.GetPinnableReference());
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
//        UnsafeWriteTo<T>(in value, destination);
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
//            var esrever = Unmanaged.Reversed(in value);
//            UnsafeWriteTo<T>(in esrever, destination);
//        }
//        else
//        {
//            UnsafeWriteTo<T>(in value, destination);
//        }
//
//        return Some(Unsafe.SizeOf<T>());
//    }
//#endregion /Writing
//
//#region Reading
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static unsafe T UnsafeReadFrom<T>(ref readonly byte bytes)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        Emit.Ldarg(nameof(bytes));
//        Emit.Unaligned(0x01);
//        Emit.Ldobj<T>();
//        return Return<T>();
//    }
//
//    [MethodImpl(MethodImplOptions.AggressiveInlining)]
//    internal static unsafe T UnsafeReadFrom<T>(ReadOnlySpan<byte> bytes)
//#if NET9_0_OR_GREATER
//        where T : allows ref struct
//#endif
//    {
//        return UnsafeReadFrom<T>(in bytes.GetPinnableReference());
//    }
//
//    extension(Unmanaged)
//    {
//        public static T Read<T>(ReadOnlySpan<byte> bytes)
//            where T : unmanaged
//#if NET9_0_OR_GREATER
//            , allows ref struct
//#endif
//        {
//            if (bytes.Length < Unsafe.SizeOf<T>())
//                throw new InvalidOperationException();
//            return UnsafeReadFrom<T>(bytes);
//        }
//
//        public static Option<T> TryRead<T>(scoped Span<byte> bytes)
//            where T : unmanaged
//        {
//            if (bytes.Length < Unsafe.SizeOf<T>())
//                return None;
//            return UnsafeReadFrom<T>(bytes);
//        }
//
//#if NET9_0_OR_GREATER
//        public static RefOption<T> TryRead<T>(ReadOnlySpan<byte> bytes, TypeConstraints.AllowsRefStruct<T> _ = default)
//            where T : unmanaged, allows ref struct
//        {
//            if (bytes.Length < Unsafe.SizeOf<T>())
//                return None;
//            return UnsafeReadFrom<T>(bytes);
//        }
//#endif
//    }
//#endregion /Reading
//
//
//#region Bit Casting
////    [MethodImpl(MethodImplOptions.AggressiveInlining)]
////    internal static TTo BitCast<TFrom, TTo>(TFrom source)
////#if NET9_0_OR_GREATER
////        where TFrom : allows ref struct
////        where TTo : allows ref struct
////#endif
////    {
////        Emit.Ldarg(nameof(source));
////        Emit.Unaligned(0x1);
////        Emit.Ldobj<TTo>();
////        return Return<TTo>();
////    }
////
////    public static bool TryBitCast<TIn, TOut>(TIn input, out TOut output)
////        where TIn : unmanaged
////        where TOut : unmanaged
////    {
////        if (Unsafe.SizeOf<TIn>() != Unsafe.SizeOf<TOut>())
////        {
////            output = default;
////            return false;
////        }
////
////        Emit.Ldarg(nameof(output));
////        Emit.Ldarg(nameof(input));
////        Emit.Unaligned(0x1);
////        Emit.Stobj<TOut>();
////        Emit.Ldc_I4_1();
////        Emit.Ret();
////        throw Unreachable();
////    }
//#endregion /BitCasting
//}