using System.Buffers;

namespace ScrubJay.Testing.Naughty;

public delegate void NaughtySequenceReaderConsumer(ref SequenceReader<byte> reader);