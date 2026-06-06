//namespace ScrubJay.Memory;
//
//public static class SpanReaderExtensions
//{
//    extension<T>(ref SpanReader<T> reader)
//    {
//
//    }
//
//    extension(ref SpanReader<byte> reader)
//    {
//        public T ReadUnmanaged<T>()
//            where T : unmanaged
//        {
//            return Bits.UnsafeRead<T>(reader.ReadMany(Unsafe.SizeOf<T>()));
//        }
//
//        public byte ReadU8() => reader.Read();
//        public sbyte ReadI8() => (sbyte)reader.Read();
//
//        public ushort ReadU16() => reader.ReadUnmanaged<ushort>();
//        public short ReadI16() => reader.ReadUnmanaged<short>();
//
//        public uint ReadU32() => reader.ReadUnmanaged<uint>();
//        public int ReadI32() => reader.ReadUnmanaged<int>();
//
//        public ulong ReadU64() => reader.ReadUnmanaged<ulong>();
//        public long ReadI64() => reader.ReadUnmanaged<long>();
//
//#if NET7_0_OR_GREATER
//        public UInt128 ReadU128() => reader.ReadUnmanaged<UInt128>();
//        public Int128 ReadI128() => reader.ReadUnmanaged<Int128>();
//#endif
//
//#if NET6_0_OR_GREATER
//        public Half ReadF16() => reader.ReadUnmanaged<Half>();
//#endif
//        public float ReadF32() => reader.ReadUnmanaged<float>();
//        public double ReadF64() => reader.ReadUnmanaged<double>();
//        public decimal ReadDecimal() => reader.ReadUnmanaged<decimal>();
//
//#region Read w/Endianness
//        public T ReadUnmanaged<T>(Endianness endianness)
//            where T : unmanaged
//        {
//            T value = Bits.UnsafeRead<T>(reader.ReadMany(Unsafe.SizeOf<T>()));
//            if (endianness.IsNonSystem)
//                Bits.Reverse(ref value);
//            return value;
//        }
//
//        public byte ReadU8(Endianness endianness) => reader.Read();
//        public sbyte ReadI8(Endianness endianness) => (sbyte)reader.Read();
//
//        public ushort ReadU16(Endianness endianness) => reader.ReadUnmanaged<ushort>(endianness);
//        public short ReadI16(Endianness endianness) => reader.ReadUnmanaged<short>(endianness);
//
//        public uint ReadU32(Endianness endianness) => reader.ReadUnmanaged<uint>(endianness);
//        public int ReadI32(Endianness endianness) => reader.ReadUnmanaged<int>(endianness);
//
//        public ulong ReadU64(Endianness endianness) => reader.ReadUnmanaged<ulong>(endianness);
//        public long ReadI64(Endianness endianness) => reader.ReadUnmanaged<long>(endianness);
//
//#if NET7_0_OR_GREATER
//        public UInt128 ReadU128(Endianness endianness) => reader.ReadUnmanaged<UInt128>(endianness);
//        public Int128 ReadI128(Endianness endianness) => reader.ReadUnmanaged<Int128>(endianness);
//#endif
//
//#if NET6_0
//        public Half ReadF16(Endianness endianness) => reader.ReadUnmanaged<Half>(endianness);
//#endif
//        public float ReadF32(Endianness endianness) => reader.ReadUnmanaged<float>(endianness);
//        public double ReadF64(Endianness endianness) => reader.ReadUnmanaged<double>(endianness);
//        public decimal ReadDecimal(Endianness endianness) => reader.ReadUnmanaged<decimal>(endianness);
//#endregion /Read w/Endianness
//    }
//}