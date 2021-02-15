using System.IO;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Projectile_Simulator
{
    /// <summary>
    /// Static class used to save objects to a file
    /// </summary>
    public static class ObjectWriter
    {
        public static void WriteJson<T>(string path, List<T> objects)
        {
            StreamWriter writer = new StreamWriter(path);

            foreach (T @object in objects)
            {
                //string data = JsonConvert.SerializeObject(@object);
                var options = new JsonSerializerOptions { Converters = { new VectorConverter() } };
                string data = JsonSerializer.Serialize(@object, @object.GetType(), options);
                writer.WriteLine(@object.GetType());
                writer.WriteLine(data);
            }

            writer.Close();
        }

        public static List<T> ReadJson<T>(string path)
        {
            StreamReader reader = new StreamReader(path);

            List<T> objects = new List<T>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line.Length > 0)
                {
                    Type dataType = Type.GetType(line);

                    //@object = JsonConvert.DeserializeObject(reader.ReadLine());

                    var options = new JsonSerializerOptions { Converters = { new VectorConverter() } };
                    dynamic @object = JsonSerializer.Deserialize(reader.ReadLine(), dataType, options);
                    objects.Add(@object);
                }    
            }

            reader.Close();

            return objects;
        }

        class VectorConverter : JsonConverter<Vector2>
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
}
