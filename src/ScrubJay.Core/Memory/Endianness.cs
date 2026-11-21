namespace ScrubJay.Memory;

/// <summary>
/// The order an architecture stores its bytes.
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Endianness"/>
[PublicAPI]
public enum Endianness
{
    /// <summary>
    /// The default host system Endianness.<br/>
    /// See <see cref="Endianness.System"/> for that exact value.
    /// </summary>
    Default = 0,

    /// <summary>
    /// Little-Endian (least-significant <see cref="byte"/> at smallest address).
    /// </summary>
    Little = 1,

    /// <summary>
    /// Big-Endian (least-significant <see cref="byte"/> at largest address).
    /// </summary>
    Big = 2,
}