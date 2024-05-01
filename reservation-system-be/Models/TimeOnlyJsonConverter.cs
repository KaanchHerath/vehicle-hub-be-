using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var timeString = reader.GetString() ?? throw new JsonException("Expected time string.");
        return TimeOnly.ParseExact(timeString, "HH:mm:ss", CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
    }
}
