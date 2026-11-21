namespace ScrubJay.Memory;

[PublicAPI]
public static class EndiannessExtensions
{
    private static readonly Endianness _systemEndianness = BitConverter.IsLittleEndian ? Endianness.Little : Endianness.Big;

    extension(Endianness)
    {
        public static Endianness operator !(Endianness endianness)
        {
            if (endianness == Endianness.Little)
                return Endianness.Big;
            if (endianness == Endianness.Big)
                return Endianness.Little;
            return BitConverter.IsLittleEndian ? Endianness.Big : Endianness.Little;
        }
    }

    extension(Endianness endianness)
    {
        /// <summary>
        /// The current host system's <see cref="Endianness"/> - either <see cref="Endianness.Little"/> or <see cref="Endianness.Big"/>
        /// </summary>
        public Endianness System => _systemEndianness;

        /// <summary>
        /// Is this <see cref="Endianness"/> the same as the host system's?
        /// </summary>
        public bool IsSystem
        {
            get
            {
                return endianness == Endianness.Default || endianness == _systemEndianness;
            }
        }

        /// <summary>
        /// Is this <see cref="Endianness"/> different than the host system's?
        /// </summary>
        public bool IsNonSystem
        {
            get
            {
                return endianness != Endianness.Default && endianness != _systemEndianness;
            }
        }

        public Endianness Resolve() => endianness switch
        {
            Endianness.Little => Endianness.Little,
            Endianness.Big => Endianness.Big,
            _ => _systemEndianness,
        };
    }
}