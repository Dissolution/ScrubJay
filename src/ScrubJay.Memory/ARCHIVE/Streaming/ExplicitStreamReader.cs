//namespace ScrubJay.Memory.Streaming;
//
//public class ExplicitStreamReader
//{
//    private readonly Stream _stream;
//
//    public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;
//
//    public Endianness DefaultEndianness { get; set; } = Endianness.System;
//
//    public ExplicitStreamReader(
//        Stream stream)
//    {
//        _stream = stream;
//    }
//
//    public Bytes ReadBytes(int count)
//    {
//        if (count <= 0)
//            return Bytes.Empty;
//        byte[] buffer = new byte[count];
//        _stream.ReadExactly(buffer);
//        return buffer;
//    }
//
//    public void Fill(Span<byte> buffer)
//    {
//        _stream.ReadExactly(buffer);
//    }
//
//    public T ReadUnmanaged<T>()
//        where T : unmanaged
//    {
//        var size = Unsafe.SizeOf<T>();
//        Span<byte> buffer = stackalloc byte[size];
//        _stream.ReadExactly(buffer);
//        return MemoryMarshal.Read<T>(buffer);
//    }
//    
//    public T ReadUnmanaged<T>(Endianness endianness)
//        where T : unmanaged
//    {
//        var size = Unsafe.SizeOf<T>();
//        Span<byte> buffer = stackalloc byte[size];
//        _stream.ReadExactly(buffer);
//        if (endianness.IsNonSystem)
//            buffer.Reverse();
//        return MemoryMarshal.Read<T>(buffer);
//    }
//}