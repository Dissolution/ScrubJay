
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace ScrubJay.Universal;

[PublicAPI]
internal struct FNV1aHasher
{
    private const uint FNV_PRIME  = 16777619U;
    private const uint FNV_OFFSET = 2166136261U;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Hash<T>(T value)
        where T : unmanaged
    {
        var h = new FNV1aHasher();
        h.Add(value);
        return h.GetI32Hash();
    }

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int HashBits<T>(ref readonly T value)
        where T : allows ref struct
    {
        var h = new FNV1aHasher();
        h.AddBits(ref value);
        return h.GetI32Hash();
    }
#endif
    
    
    private uint _hash;

    public FNV1aHasher()
    {
        _hash = FNV_OFFSET;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(scoped ReadOnlySpan<byte> data)
    {
        unchecked
        {
            ref byte current = ref MemoryMarshal.GetReference(data);
            int length = data.Length;

            // 4-byte chunks
            while (length >= 4)
            {
                uint slice = Unsafe.ReadUnaligned<uint>(ref current);

                // normalize to little-endian
                if (!BitConverter.IsLittleEndian)
                {
                    slice = BinaryPrimitives.ReverseEndianness(slice);
                }

                _hash ^= slice;
                _hash *= FNV_PRIME;

                current = ref Unsafe.Add(ref current, 4);
                length -= 4;
            }

            // remaining bytes
            while (length > 0)
            {
                _hash ^= current;
                _hash *= FNV_PRIME;

                current = ref Unsafe.Add(ref current, 1);
                length--;
            }
        }
    }

    public void Add(byte u8)
    {
        unchecked
        {
            _hash ^= u8;
            _hash *= FNV_PRIME;
        }
    }

    public void Add(sbyte i8) => Add((byte)i8);
    
    public void Add(short i16)
    {
        Span<byte> buffer = stackalloc byte[sizeof(short)];
        BinaryPrimitives.WriteInt16LittleEndian(buffer, i16);
        Add(buffer);
    }

    public void Add(ushort u16)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ushort)];
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, u16);
        Add(buffer);
    }

    public void Add(int i32)
    {
        Span<byte> buffer = stackalloc byte[sizeof(int)];
        BinaryPrimitives.WriteInt32LittleEndian(buffer, i32);
        Add(buffer);
    }

    public void Add(uint u32)
    {
        Span<byte> buffer = stackalloc byte[sizeof(uint)];
        BinaryPrimitives.WriteUInt32LittleEndian(buffer, u32);
        Add(buffer);
    }

    public void Add(long i64)
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];
        BinaryPrimitives.WriteInt64LittleEndian(buffer, i64);
        Add(buffer);
    }

    public void Add(ulong u64)
    {
        Span<byte> buffer = stackalloc byte[sizeof(ulong)];
        BinaryPrimitives.WriteUInt64LittleEndian(buffer, u64);
        Add(buffer);
    }

    public void Add(nint nint)  => Add((long)nint);
    
    public void Add(nuint nuint) => Add((ulong)nuint);

    public void Add(float f32)
    {
        Span<byte> buffer = stackalloc byte[sizeof(float)];
        BinaryPrimitives.WriteSingleLittleEndian(buffer, f32);
        Add(buffer);
    }
    
    public void Add(double f64)
    {
        Span<byte> buffer = stackalloc byte[sizeof(double)];
        BinaryPrimitives.WriteDoubleLittleEndian(buffer, f64);
        Add(buffer);
    }

    public void Add(decimal dec)
    {
        Span<int> bits = stackalloc int[sizeof(decimal)];
        decimal.GetBits(dec, bits);

        Add(bits[0]);
        Add(bits[1]);
        Add(bits[2]);
        Add(bits[3]);
    }

    public void Add(bool boolean) => Add((byte)(boolean ? 1 : 0));
    
    public void Add(char ch)
    {
        Span<byte> buffer = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16LittleEndian(buffer, ch);
        Add(buffer);
    }
    
    public void Add(string? str)
    {
        if (str is null)
        {
            Add(0);
            return;
        }

        Add(str.Length);

        var bytes = MemoryMarshal.AsBytes(str.AsSpan());
        Add(bytes);
    }

    public void Add(ReadOnlySpan<char> value)
    {
        var bytes = MemoryMarshal.AsBytes(value);
        Add(bytes);
    }

    
    public void Add(Guid guid)
    {
        Span<byte> buffer = stackalloc byte[16];
        guid.TryWriteBytes(buffer);
        Add(buffer);
    }
    
    public void AddEnum<TEnum>(TEnum @enum)
        where TEnum : struct, Enum
    {
        Add(Unsafe.As<TEnum, ulong>(ref @enum));
    }


#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AddBits<T>(ref readonly T value)
        where T : allows ref struct
    {
        ref T mutable = ref Unsafe.AsRef(in value);
        ref byte first = ref Unsafe.As<T, byte>(ref mutable);

        var span = MemoryMarshal.CreateReadOnlySpan(
            ref first,
            Unsafe.SizeOf<T>());

        Add(span);
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(T value)
        where T : unmanaged
    {
        AddBits(ref value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add<T>(T value, IEqualityComparer<T> comparer)
    {
        Add(comparer.GetHashCode(value));
    }

    
    
 
    public void Add(DateTime value)
    {
        Add(value.Ticks);
        Add((int)value.Kind);
    }

    public void Add(DateTimeOffset value)
    {
        Add(value.Ticks);
        Add(value.Offset.Ticks);
    }

    public void Add(TimeSpan value)
    {
        Add(value.Ticks);
    }

    // ============================================================
    // Finalization
    // ============================================================

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetI32Hash() => unchecked((int)_hash);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint GetU32Hash() => _hash;
}