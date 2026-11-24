using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fallout.Common.Tools.GitVersion;

/// <summary>
/// Deserializes a JSON string or a JSON number as a C# <see langword="string"/>.
/// GitVersion 6 emits some string fields as bare numbers.
/// </summary>
internal sealed class NumberToStringJsonConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Number => reader.TryGetInt64(out var number)
                ? number.ToString(CultureInfo.InvariantCulture)
                : reader.GetDouble().ToString(CultureInfo.InvariantCulture),
            JsonTokenType.Null => null,
            _ => throw new JsonException($"Unexpected token type {reader.TokenType} when deserializing string.")
        };
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
        }
        else
        {
            writer.WriteStringValue(value);
        }
    }
}
