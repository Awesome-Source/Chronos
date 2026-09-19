using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chronos.Asp.Json
{
    /// <summary>
    /// Serializes TimeSpan as a plain number of total seconds instead of the default
    /// ISO-8601 duration string, matching the frontend's duration-in-seconds convention.
    /// </summary>
    public class TimeSpanSecondsJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.FromSeconds(reader.GetDouble());
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.TotalSeconds);
        }
    }
}
