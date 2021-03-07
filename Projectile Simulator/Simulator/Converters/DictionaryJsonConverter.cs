using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Simulator.Simulation;

namespace Simulator.Converters
{
    /*
    class DictionaryJsonConverter : JsonConverter<Dictionary<ITrigger, Stopwatch.StopwatchInput>>
    {
        public override Dictionary<ITrigger, Stopwatch.StopwatchInput> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            // Split into components
            string[] components = value.Split(',');
            return new Vector2(float.Parse(components[0]), float.Parse(components[1]));
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<ITrigger, Stopwatch.StopwatchInput> value, JsonSerializerOptions options)
        {
            foreach (var pair in value)
            {
                writer.WriteStringValue(pair.Key.ToString() + ':' + value.Y.ToString());
            }
        }
    }
    */
}
