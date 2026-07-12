namespace ScrubJay.Debugging;

internal static class InternalExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetValue<T>([AllowNull, NotNullWhen(true)] this Nullable<T> nullable, [MaybeNullWhen(false)] out T value)
        where T : struct
    {
        if (nullable.HasValue)
        {
            value = nullable.GetValueOrDefault();
            return true;
        }
        value = default;
        return false;
    }

    extension(byte u8)
    {
        public string ToBinaryString() => string.Create(8, u8, static (span, b) =>
        {
            span[0] = ((b & 0b00000001) != 0) ? '1' : '0';
            span[1] = ((b & 0b00000010) != 0) ? '1' : '0';
            span[2] = ((b & 0b00000100) != 0) ? '1' : '0';
            span[3] = ((b & 0b00001000) != 0) ? '1' : '0';
            span[4] = ((b & 0b00010000) != 0) ? '1' : '0';
            span[5] = ((b & 0b00100000) != 0) ? '1' : '0';
            span[6] = ((b & 0b01000000) != 0) ? '1' : '0';
            span[7] = ((b & 0b10000000) != 0) ? '1' : '0';
        });
    }
}