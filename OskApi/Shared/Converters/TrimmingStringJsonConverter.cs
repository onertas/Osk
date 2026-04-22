using System.Text.Json;
using System.Text.Json.Serialization;

namespace OskApi.Shared.Converters;

/// <summary>
/// JSON deserializasyonu sırasında tüm string alanlarını otomatik trim eder.
/// Program.cs'te AddJsonOptions içinde global olarak kaydedilir.
/// </summary>
public class TrimmingStringJsonConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value?.Trim();
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
