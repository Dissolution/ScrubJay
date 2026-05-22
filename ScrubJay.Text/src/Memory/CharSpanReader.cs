namespace ScrubJay.Text.Memory;

[PublicAPI]
public ref struct SpanReader<T>
{
    [DoesNotReturn]
    [StackTraceHidden]
    internal static void ThrowCannotReadException(int count)
    {
        Debug.Assert(count > 0);
        throw new InvalidOperationException($"Could not read {count} items");
    }

    private readonly ReadOnlySpan<T> _span;
    private int _position;

    public readonly int Position => _position;

    public SpanReader(ReadOnlySpan<T> span)
    {
        _span = span;
        _position = 0;
    }

    public T Read()
    {
        int pos = _position;
        if (pos >= _span.Length)
            ThrowCannotReadException(1);

        _position = pos + 1;
        return _span[pos];
    }

    public ReadOnlySpan<T> ReadMany(int count)
    {
        if (count <= 0)
            return ReadOnlySpan<T>.Empty;
        int pos = _position;
        int newPos = pos + count;
        if (newPos >= _span.Length)
            ThrowCannotReadException(count);

        _position = newPos;
        return _span.Slice(pos, count);
    }
}

[PublicAPI]
public ref struct SpanWriter<T>
{
    private Span<T> _span;
    private int _position;

    public SpanWriter(Span<T> span)
    {
        _span = span;
        _position = 0;
    }
}

public static class SpanReaderExtensions
{
    extension<T>(ref SpanReader<T> reader)
    {

    }

    extension(ref SpanReader<byte> reader)
    {
        public T ReadUnmanaged<T>()
            where T : unmanaged
        {
            return Unsafe.ReadUnaligned<T>(
                ref MemoryMarshal.GetReference(
                    reader.ReadMany(
                        Unsafe.SizeOf<T>()
                    )
                )
            );
        }

        public byte ReadU8() => reader.Read();
        public sbyte ReadI8() => (sbyte)reader.Read();

        public ushort ReadU16() => reader.ReadUnmanaged<ushort>();
        public short ReadI16() => reader.ReadUnmanaged<short>();

        public uint ReadU32() => reader.ReadUnmanaged<uint>();
        public int ReadI32() => reader.ReadUnmanaged<int>();

        public ulong ReadU64() => reader.ReadUnmanaged<ulong>();
        public long ReadI64() => reader.ReadUnmanaged<long>();

        public float ReadF32() => reader.ReadUnmanaged<float>();
        public double ReadF64() => reader.ReadUnmanaged<double>();
        public decimal ReadDecimal() => reader.ReadUnmanaged<decimal>();
    }
}

public static class SpanWriterExtensions
{
    extension<T>(ref SpanWriter<T> writer)
    {

    }
}