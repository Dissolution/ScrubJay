//using System.Buffers.Binary;
//using ScrubJay.Memory.ARCHIVE.Utilities;
//
//namespace ScrubJay.Memory.ARCHIVE.Extensions;
//
///// <summary>
///// Extensions on <see cref="SpanReader{T}">SpanReader&lt;byte&gt;</see>
///// </summary>
//[PublicAPI]
//public static class ByteSpanReaderExtensions
//{
//    extension(ref SpanReader<byte> reader)
//    {
//#region Read
//#region Primitives
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public byte ReadU8()
//        {
//            return reader.Take();
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public byte ReadU8(Endianness endianness)
//        {
//            return reader.Take();
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public sbyte ReadI8()
//        {
//            return (sbyte)reader.Take();
//        }
//
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public sbyte ReadI8(Endianness endianness)
//        {
//            return (sbyte)reader.Take();
//        }
//
//        public short ReadI16()
//        {
//            var bytes = reader.TakeMany(sizeof(short));
//            short i16 = Bits.UnsafeReadFrom<short>(bytes);
//            return i16;
//        }
//
//        public short ReadI16(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(sizeof(short));
//            short i16 = Bits.UnsafeReadFrom<short>(bytes);
//            if (endianness.IsSystem)
//                return i16;
//            return BinaryPrimitives.ReverseEndianness(i16);
//        }
//
//        public ushort ReadU16()
//        {
//            var bytes = reader.TakeMany(sizeof(ushort));
//            ushort u16 = Bits.UnsafeReadFrom<ushort>(bytes);
//            return u16;
//        }
//
//        public ushort ReadU16(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(sizeof(ushort));
//            ushort u16 = Bits.UnsafeReadFrom<ushort>(bytes);
//            if (endianness.IsSystem)
//                return u16;
//            return BinaryPrimitives.ReverseEndianness(u16);
//        }
//
//        public int ReadI32()
//        {
//            var bytes = reader.TakeMany(sizeof(int));
//            int i32 = Bits.UnsafeReadFrom<int>(bytes);
//            return i32;
//        }
//
//        public int ReadI32(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(sizeof(int));
//            int i32 = Bits.UnsafeReadFrom<int>(bytes);
//            if (endianness.IsSystem)
//                return i32;
//            return BinaryPrimitives.ReverseEndianness(i32);
//        }
//
//        public uint ReadU32()
//        {
//            var bytes = reader.TakeMany(sizeof(uint));
//            uint u32 = Bits.UnsafeReadFrom<uint>(bytes);
//            return u32;
//        }
//
//        public uint ReadU32(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(sizeof(uint));
//            uint u32 = Bits.UnsafeReadFrom<uint>(bytes);
//            if (endianness.IsSystem)
//                return u32;
//            return BinaryPrimitives.ReverseEndianness(u32);
//        }
//
//        public long ReadI64()
//        {
//            var bytes = reader.TakeMany(sizeof(long));
//            long i64 = Bits.UnsafeReadFrom<long>(bytes);
//            return i64;
//        }
//
//        public long ReadI64(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(sizeof(long));
//            long i64 = Bits.UnsafeReadFrom<long>(bytes);
//            if (endianness.IsSystem)
//                return i64;
//            return BinaryPrimitives.ReverseEndianness(i64);
//        }
//
//        public ulong ReadU64()
//        {
//            var bytes = reader.TakeMany(sizeof(ulong));
//            ulong u64 = Bits.UnsafeReadFrom<ulong>(bytes);
//            return u64;
//        }
//
//        public ulong ReadU64(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(sizeof(ulong));
//            ulong u64 = Bits.UnsafeReadFrom<ulong>(bytes);
//            if (endianness.IsSystem)
//                return u64;
//            return BinaryPrimitives.ReverseEndianness(u64);
//        }
//
//#if NET6_0_OR_GREATER
//        public Half ReadF16()
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<Half>());
//            Half f16 = Bits.UnsafeReadFrom<Half>(bytes);
//            return f16;
//        }
//
//        public Half ReadF16(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<Half>());
//            Half f16 = Bits.UnsafeReadFrom<Half>(bytes);
//            if (endianness.IsNonSystem)
//                Unmanaged.Reverse(ref f16);
//            return f16;
//        }
//#endif
//
//        public float ReadF32()
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<float>());
//            float f32 = Bits.UnsafeReadFrom<float>(bytes);
//            return f32;
//        }
//
//        public float ReadF32(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<float>());
//            float f32 = Bits.UnsafeReadFrom<float>(bytes);
//            if (endianness.IsNonSystem)
//                Unmanaged.Reverse(ref f32);
//            return f32;
//        }
//
//        public double ReadF64()
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<double>());
//            double f64 = Bits.UnsafeReadFrom<double>(bytes);
//            return f64;
//        }
//
//        public double ReadF64(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<double>());
//            double f64 = Bits.UnsafeReadFrom<double>(bytes);
//            if (endianness.IsNonSystem)
//                Unmanaged.Reverse(ref f64);
//            return f64;
//        }
//
//        public decimal ReadDecimal()
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<decimal>());
//            decimal dec = Bits.UnsafeReadFrom<decimal>(bytes);
//            return dec;
//        }
//
//        public decimal ReadDecimal(Endianness endianness)
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<decimal>());
//            decimal dec = Bits.UnsafeReadFrom<decimal>(bytes);
//            if (endianness.IsNonSystem)
//                Unmanaged.Reverse(ref dec);
//            return dec;
//        }
//
//        public bool ReadBool()
//        {
//            return reader.Take() != 0;
//        }
//
//        public T ReadUnmanaged<T>()
//            where T : unmanaged
//#if NET9_0_OR_GREATER
//            , allows ref struct
//#endif
//
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<T>());
//            T value = Bits.UnsafeReadFrom<T>(bytes);
//            return value;
//        }
//
//        public T ReadUnmanaged<T>(Endianness endianness)
//            where T : unmanaged
//#if NET9_0_OR_GREATER
//            , allows ref struct
//#endif
//
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<T>());
//            T value = Bits.UnsafeReadFrom<T>(bytes);
//            if (endianness.IsNonSystem)
//                Unmanaged.Reverse(ref value);
//            return value;
//        }
//
//        public E ReadEnum<E>()
//            where E : struct, Enum
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<E>());
//            E value = Bits.UnsafeReadFrom<E>(bytes);
//            return value;
//        }
//
//        public E ReadEnum<E>(Endianness endianness)
//            where E : struct, Enum
//        {
//            var bytes = reader.TakeMany(Unsafe.SizeOf<E>());
//            E value = Bits.UnsafeReadFrom<E>(bytes);
//            if (endianness.IsNonSystem)
//                Bits.Reverse(ref value);
//            return value;
//        }
//#endregion
//
//#region Text
//        public char ReadChar()
//        {
//            return Unmanaged.Read<char>(reader.TakeMany(sizeof(char)));
//        }
//
//        public char ReadChar(Endianness endianness)
//        {
//            char ch = Unmanaged.Read<char>(reader.TakeMany(sizeof(char)));
//            if (endianness.IsNonSystem)
//                Bits.Reverse(ref ch);
//            return ch;
//        }
//
//        public string ReadString(int length, Encoding? encoding = null)
//        {
//            if (length <= 0)
//                return string.Empty;
//#if NETFRAMEWORK || NETSTANDARD2_0
//            var bytes = reader.TakeManyToArray(length);
//            return (encoding ?? Encoding.UTF8).GetString(bytes);
//#else
//            var bytes = reader.TakeMany(length);
//            return (encoding ?? Encoding.UTF8).GetString(bytes);
//#endif
//        }
//
//        public string ReadString(int length, Endianness endianness, Encoding? encoding = null)
//        {
//            if (length <= 0)
//                return string.Empty;
//            if (endianness.IsSystem)
//            {
//#if NETFRAMEWORK || NETSTANDARD2_0
//                var bytes = reader.TakeManyToArray(length);
//                return (encoding ?? Encoding.UTF8).GetString(bytes);
//#else
//            var bytes = reader.TakeMany(length);
//            return (encoding ?? Encoding.UTF8).GetString(bytes);
//#endif
//            }
//            else
//            {
//#if NETFRAMEWORK || NETSTANDARD2_0
//                byte[] bytes = new byte[length];
//#else
//                Span<byte> bytes = length <= 64 ? stackalloc byte[length] : new byte[length];
//#endif
//                reader.Fill(bytes);
//                bytes.Reverse();
//                return (encoding ?? Encoding.UTF8).GetString(bytes);
//            }
//        }
//
//        public string ReadString(
//            StringEncodingAffix fix,
//            Encoding? encoding = null)
//            => ReadString(ref reader, fix, Endianness.System, encoding);
//
//        public string ReadString(
//            StringEncodingAffix fix,
//            Endianness endianness,
//            Encoding? encoding = null)
//        {
//            switch (fix)
//            {
//                case StringEncodingAffix.SevenBitEncodedLenPrefix:
//                {
//                    int len = reader.Read7BitEncodedI32();
//                    return reader.ReadString(len, endianness, encoding);
//                }
//                case StringEncodingAffix.U8Prefix:
//                {
//                    byte len = ReadU8(ref reader);
//                    return reader.ReadString(len, endianness, encoding);
//                }
//                case StringEncodingAffix.U16Prefix:
//                {
//                    ushort len = ReadU16(ref reader);
//                    return reader.ReadString(len, endianness, encoding);
//                }
//                case StringEncodingAffix.U32Prefix:
//                {
//                    uint len = ReadU32(ref reader);
//                    if (len > (uint)int.MaxValue)
//                        throw new InvalidOperationException();
//                    return reader.ReadString((int)len, endianness, encoding);
//                }
//                case StringEncodingAffix.U64Prefix:
//                {
//                    ulong len = ReadU64(ref reader);
//                    if (len > (ulong)int.MaxValue)
//                        throw new InvalidOperationException();
//                    return reader.ReadString((int)len, endianness, encoding);
//                }
//                case StringEncodingAffix.I8Prefix:
//                {
//                    sbyte len = ReadI8(ref reader);
//                    return reader.ReadString(len, endianness, encoding);
//                }
//                case StringEncodingAffix.I16Prefix:
//                {
//                    short len = ReadI16(ref reader);
//                    return reader.ReadString(len, endianness, encoding);
//                }
//                case StringEncodingAffix.I32Prefix:
//                {
//                    int len = ReadI32(ref reader);
//                    return reader.ReadString(len, endianness, encoding);
//                }
//                case StringEncodingAffix.I64Prefix:
//                {
//                    long len = ReadI64(ref reader);
//                    if (len > (long)int.MaxValue)
//                        throw new InvalidOperationException();
//                    return reader.ReadString((int)len, endianness, encoding);
//                }
//                case StringEncodingAffix.NullTerminated:
//                {
//                    encoding ??= Encoding.UTF8;
//                    int charSize = encoding.GetByteCount("J");
//                    Span<byte> nullChar = stackalloc byte[charSize];
//
//                    var bytes = reader.TakeUntilMatching(nullChar, chunk: true)
//#if NETFRAMEWORK || NETSTANDARD2_0
//                            .ToArray()
//#endif
//                        ;
//
//                    string str = encoding.GetString(bytes);
//                    return str;
//                }
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//#endregion
//
//#region Special Encodings
//        public int Read7BitEncodedI32()
//        {
//            uint result = 0;
//            byte u8;
//
//            const int MAX_BYTES_WITHOUT_OVERFLOW = 4;
//            for (int shift = 0; shift < MAX_BYTES_WITHOUT_OVERFLOW * 7; shift += 7)
//            {
//                // ReadU8 handles end of stream
//                u8 = ReadU8(ref reader);
//                result |= (u8 & 0x7Fu) << shift;
//
//                if (u8 <= 0x7Fu)
//                {
//                    return (int)result; // early exit
//                }
//            }
//
//            u8 = ReadU8(ref reader);
//            if (u8 > 0b_1111u)
//            {
//                throw new InvalidOperationException();
//            }
//
//            result |= (uint)u8 << MAX_BYTES_WITHOUT_OVERFLOW * 7;
//            return (int)result;
//        }
//
//        public long Read7BitEncodedI64()
//        {
//            ulong result = 0;
//            byte u8;
//
//            const int MAX_BYTES_WITHOUT_OVERFLOW = 9;
//            for (int shift = 0; shift < MAX_BYTES_WITHOUT_OVERFLOW * 7; shift += 7)
//            {
//                // ReadU8 handles end of stream cases for us.
//                u8 = ReadU8(ref reader);
//                result |= (u8 & 0x7Ful) << shift;
//
//                if (u8 <= 0x7Fu)
//                {
//                    return (long)result; // early exit
//                }
//            }
//
//            u8 = ReadU8(ref reader);
//            if (u8 > 0b_1u)
//            {
//                throw new InvalidOperationException();
//            }
//
//            result |= (ulong)u8 << MAX_BYTES_WITHOUT_OVERFLOW * 7;
//            return (long)result;
//        }
//#endregion
//
//#region Time
//        public TimeSpan ReadTimeSpan(TimeEncodingAffix fix)
//        {
//            switch (fix)
//            {
//                case TimeEncodingAffix.Ticks:
//                {
//                    long ticks = ReadI64(ref reader);
//                    return new TimeSpan(ticks);
//                }
//                case TimeEncodingAffix.TimeU32:
//                {
//                    uint seconds = ReadU32(ref reader);
//                    return TimeSpan.FromSeconds(seconds);
//                }
//                case TimeEncodingAffix.TimeU64:
//                {
//                    ulong seconds = ReadU64(ref reader);
//                    return TimeSpan.FromSeconds(seconds);
//                }
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//
//        public DateTime ReadDateTime(
//            TimeEncodingAffix fix)
//        {
//            switch (fix)
//            {
//                case TimeEncodingAffix.Ticks:
//                {
//                    long ticks = ReadI64(ref reader);
//                    return new DateTime(ticks);
//                }
//                case TimeEncodingAffix.TimeU32:
//                {
//                    uint seconds = ReadU32(ref reader);
//                    return TimeEncodingAffix.OriginDateTime.AddSeconds(seconds);
//                }
//                case TimeEncodingAffix.TimeU64:
//                {
//                    ulong seconds = ReadU64(ref reader);
//                    return TimeEncodingAffix.OriginDateTime.AddSeconds(seconds);
//                }
//                default:
//                    throw new ArgumentOutOfRangeException();
//            }
//        }
//#endregion
//#endregion
//
//    } // end extension block
//}