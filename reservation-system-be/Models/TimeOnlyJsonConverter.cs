using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeOnlyJsonConverter : JsonConverter<DateTime>
{
    private const string TimeFormat = "HH:mm:ss";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string timeAsString = reader.GetString();
        TimeOnly time = TimeOnly.ParseExact(timeAsString, TimeFormat, System.Globalization.CultureInfo.InvariantCulture);
        return DateTime.Today.Add(time.ToTimeSpan()); // Combines the TimeOnly part with today's date, adjust based on your needs
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        TimeOnly timeOnly = TimeOnly.FromDateTime(value);
        writer.WriteStringValue(timeOnly.ToString(TimeFormat));
    }
}
