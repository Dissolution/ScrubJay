using System.Text.Json;
using System.Text.Json.Serialization;
using ScrubJay.Functional;

namespace ScrubJay.Json.Functional;

/// <summary>
/// A <see cref="JsonConverter{T}"/> that works with <see cref="Option{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
[PublicAPI]
public sealed class OptionJsonConverter<T> : JsonConverter<Option<T>>
{
    public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        var valueConverter = options.GetConverter<T>();
        if (valueConverter is null)
            throw new JsonException($"No JsonConverter found for type {typeof(T)}");
        
        var value = valueConverter.Read(ref reader, options);
        return Some(value);
    }

    public override void Write(Utf8JsonWriter writer, Option<T> option, JsonSerializerOptions options)
    {
        if (option.IsSome(out var value))
        {
            var valueConverter = options.GetConverter<T>()!;
            valueConverter.Write(writer, value, options);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}