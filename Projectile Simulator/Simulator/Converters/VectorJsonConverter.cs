using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Simulator.Converters
{
    public class VectorJsonConverter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();
            string[] components = value.Split(',');
            return new Vector2(float.Parse(components[0]), float.Parse(components[1]));
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.X.ToString() + ',' + value.Y.ToString());
        }
    }
}
