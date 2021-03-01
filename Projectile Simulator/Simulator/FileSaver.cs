using System.IO;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Simulator.Converters;

namespace Simulator
{
    /// <summary>
    /// Static class used to save objects to a file
    /// </summary>
    public static class FileSaver
    {
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            Converters =
            {
                new Vector2JsonConverter(),
                new Vector3JsonConverter()
            }
        };

        public static void WriteJson<T>(string path, List<T> objects)
        {
            StreamWriter writer = new StreamWriter(path);

            foreach (T @object in objects)
            {
                if (@object is IPersistent)
                {
                    string data = JsonSerializer.Serialize(@object, @object.GetType(), options);
                    writer.WriteLine(@object.GetType());
                    writer.WriteLine(data);
                }
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
                    dynamic @object = JsonSerializer.Deserialize(reader.ReadLine(), dataType, options);
                    objects.Add(@object);
                }    
            }

            reader.Close();

            return objects;
        } 
    }
}
