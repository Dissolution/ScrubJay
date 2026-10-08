namespace ScrubJay.Memory;

[PublicAPI]
public static class ByteSpanReaderExtensions
{
    extension(ref SpanReader<byte> reader)
    {
        public bool TryTake<U>(out U value)
            where U : unmanaged
        {
            if (reader.TryTakeMany(Unsafe.SizeOf<U>(), out var span))
            {
                value = BitHelper.Notsafe.Read<U>(span);
                return true;
            }
            
            value = default;
            return false;
        }
    }
}