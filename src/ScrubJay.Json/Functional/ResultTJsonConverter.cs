//using System.Text.Json;
//using System.Text.Json.Serialization;
//
//namespace ScrubJay.Json.Functional;
//
///// <summary>
///// A <see cref="JsonConverter{T}"/> that works with <see cref="Result{T}"/>
///// </summary>
//[PublicAPI]
//public sealed class ResultJsonConverter<T> : JsonConverter<Result<T>>
//{
//    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        if (reader.TokenType != JsonTokenType.StartObject)
//            throw new JsonException();
//        _ = reader.Read();
//
//        if (reader.TokenType != JsonTokenType.PropertyName)
//            throw new JsonException();
//
//        Result<T> result;
//
//        if (reader.ValueSpan.SequenceEqual("ok"u8) && reader.Read())
//        {
//            var converter = options.GetConverter<T>()!;
//            var value = converter.Read(ref reader, options);
//            result = Result<T>.Ok(value!);
//        }
//        else if (reader.ValueSpan.SequenceEqual("error"u8) && reader.Read())
//        {
//            var converter = options.GetConverter<Exception>()!;
//            var error = converter.Read(ref reader, options);
//            result = Result<T>.Error(error!);
//        }
//        else
//        {
//            throw new JsonException("Property was not named `ok` nor `error`");
//        }
//
//        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
//            throw new JsonException("Expected endobject");
//
//        return result;
//    }
//
//    public override void Write(Utf8JsonWriter writer, Result<T> result, JsonSerializerOptions options)
//    {
//        writer.WriteStartObject();
//
//        if (result.IsOk(out var ok, out var error))
//        {
//            writer.WritePropertyName("ok");
//            var converter = options.GetConverter<T>()!;
//            converter.Write(writer, ok, options);
//        }
//        else
//        {
//            writer.WritePropertyName("error");
//            var converter = options.GetConverter<Exception>()!;
//            converter.Write(writer, error, options);
//        }
//
//        writer.WriteEndObject();
//    }
//}