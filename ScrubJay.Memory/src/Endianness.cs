namespace ScrubJay.Memory;

/// <summary>
/// The order of bytes.
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Endianness"/>
[PublicAPI]
public enum Endianness
{
    /// <summary>
    /// Little-Endian (least-significant <see cref="byte"/> at smallest address).
    /// </summary>
    Little,

    /// <summary>
    /// Big-Endian (least-significant <see cref="byte"/> at largest address).
    /// </summary>
    Big,
}

[PublicAPI]
public static class EndiannessExtensions
{
    internal static readonly Endianness SystemEndianness = BitConverter.IsLittleEndian ?  Endianness.Little : Endianness.Big;
    
    extension(Endianness)
    {
        public static Endianness System => SystemEndianness;
        
        public static Endianness operator !(Endianness endianness)
        {
            if (endianness == Endianness.Little)
                return Endianness.Big;
            if (endianness == Endianness.Big)
                return Endianness.Little;
            if (BitConverter.IsLittleEndian)
                return Endianness.Big;
            return Endianness.Little;
        }
    }

    extension(Endianness endianness)
    {
        public bool IsLittle => endianness == Endianness.Little;
        
        public bool IsBig => endianness == Endianness.Big;

        public bool IsSystem => endianness == SystemEndianness;

        public bool IsNonSystem => endianness != SystemEndianness;
    }
}