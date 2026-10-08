namespace ScrubJay.Memory;

/// <summary>
/// The order of bytes.
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Endianness"/>
[PublicAPI]
public enum Endianness
{
    /// <summary>
    /// Unchanged from however the System treats the bytes.
    /// </summary>
    System = 0,
    
    /// <summary>
    /// Little-Endian (least-significant <see cref="byte"/> at smallest address).
    /// </summary>
    Little = 1,

    /// <summary>
    /// Big-Endian (least-significant <see cref="byte"/> at largest address).
    /// </summary>
    Big = 2,
}

[PublicAPI]
public static class EndiannessExtensions
{
    internal static readonly Endianness SystemEndianness = BitConverter.IsLittleEndian ?  Endianness.Little : Endianness.Big;
    
    extension(Endianness)
    {
        public static Endianness operator !(Endianness endianness)
        {
            if (endianness == Endianness.Little)
                return Endianness.Big;
            if (endianness == Endianness.Big)
                return Endianness.Little;
            return BitConverter.IsLittleEndian ? Endianness.Big :  Endianness.Little;
        }
    }

    extension(Endianness endianness)
    {
        public bool IsSystem
        {
            get
            {
                return endianness == Endianness.System || endianness == SystemEndianness;
            }
        }

        public bool IsNonSystem
        {
            get
            {
                return endianness != Endianness.System && endianness != SystemEndianness;
            }
        }
    }
}