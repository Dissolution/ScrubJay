using System.Text.Json;
using System.Text.Json.Serialization;

namespace ScrubJay.Json.Extensions;

[PublicAPI]
public static class JsonExtensions
{
    extension(JsonSerializerOptions options)
    {
        public JsonConverter<T>? GetConverter<T>()
        {
            var converter = options.GetConverter(typeof(T));
            if (converter is JsonConverter<T> typed)
                return typed;
            return null;
        }
    }

    public static T? Read<T>(this JsonConverter<T> converter, ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        return converter.Read(ref reader, typeof(T), options);
    }
}