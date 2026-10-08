using System.Text.Json;
using System.Text.Json.Serialization;
using ScrubJay.Functional;

namespace ScrubJay.Json.Functional;

/// <summary>
/// A <see cref="JsonConverterFactory"/> that creates <see cref="OptionJsonConverter{T}"/> instances
/// </summary>
[PublicAPI]
public sealed class OptionJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType &&
            typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converter = Activator.CreateInstance(typeof(OptionJsonConverter<>).MakeGenericType(typeToConvert)) as JsonConverter;
        return converter;
    }
}