namespace ScrubJay.Text.Memory;

/// <summary>
/// The order of bytes.
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Endianness"/>
[PublicAPI]
public enum Endianness
{
    /// <summary>
    /// The default endianness of the system (per <see cref="BitConverter.IsLittleEndian"/>) (does not change <see cref="byte"/> order).
    /// </summary>
    System,

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
    extension(Endianness)
    {
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
        
        public bool IsSystem => endianness is < Endianness.Little or > Endianness.Big;

        public bool IsNonSystem => BitConverter.IsLittleEndian ? endianness != Endianness.Little : endianness != Endianness.Big;
        
        public Endianness Resolve()
        {
            if (endianness == Endianness.Little)
                return Endianness.Little;
            if (endianness == Endianness.Big)
                return Endianness.Big;
            if (BitConverter.IsLittleEndian)
                return Endianness.Little;
            return Endianness.Big;
        }
    }
}