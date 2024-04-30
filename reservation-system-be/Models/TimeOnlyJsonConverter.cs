using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeOnlyJsonConverter : JsonConverter<DateTime>
{
    private const string TimeFormat = "HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string timeAsString = reader.GetString() ?? throw new JsonException("Time string is null.");
        TimeOnly time = TimeOnly.ParseExact(timeAsString, TimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        return DateTime.Parse(time.ToString());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        TimeOnly timeOnly = TimeOnly.FromDateTime(value);
        writer.WriteStringValue(timeOnly.ToString(TimeFormat));
    }
}
