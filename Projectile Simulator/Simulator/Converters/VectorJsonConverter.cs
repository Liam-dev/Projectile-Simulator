﻿using Microsoft.Xna.Framework;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Simulator.Converters
{
    /// <summary>
    /// Converts Vector2 to JSON for serialization.
    /// </summary>
    public class Vector2JsonConverter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            // Split into components
            string[] components = value.Split(',');
            return new Vector2(float.Parse(components[0]), float.Parse(components[1]));
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.X.ToString() + ',' + value.Y.ToString());
        }
    }

    /// <summary>
    /// Converts Vector3 to JSON for serialization.
    /// </summary>
    public class Vector3JsonConverter : JsonConverter<Vector3>
    {
        public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            // Split into components
            string[] components = value.Split(',');
            return new Vector3(float.Parse(components[0]), float.Parse(components[1]), float.Parse(components[2]));
        }

        public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.X.ToString() + ',' + value.Y.ToString() + ',' + value.Z.ToString());
        }
    }
}