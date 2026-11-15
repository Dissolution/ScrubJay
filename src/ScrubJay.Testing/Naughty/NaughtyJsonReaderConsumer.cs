using System.Text.Json;

namespace ScrubJay.Testing.Naughty;

public delegate void NaughtyJsonReaderConsumer(ref Utf8JsonReader reader);