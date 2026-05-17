using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScrubJay.Functional.Json;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that works on <see cref="Unit"/>
/// </summary>
[PublicAPI]
public sealed class NoneJsonConverter : JsonConverter<IMPL.None>
{
    public override IMPL.None Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Null)
            throw new JsonException();
        return default;
    }

    public override void Write(Utf8JsonWriter writer, IMPL.None _, JsonSerializerOptions options)
    {
        writer.WriteNullValue();
    }
}