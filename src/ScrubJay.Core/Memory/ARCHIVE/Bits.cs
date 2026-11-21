


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