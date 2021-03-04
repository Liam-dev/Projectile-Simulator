using System.IO;
using System.Collections.Generic;
using System;
using System.Text.Json;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Xna.Framework;
using Simulator.Converters;


namespace Simulator
{
    /// <summary>
    /// Static class used to serialize objects using JSON encoding and save to a file.
    /// </summary>
    public static class FileSaver
    {
        // Custom converters for XNA Vector structures.
        
        private static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            Converters =
            {
                new Vector2JsonConverter(),
                new Vector3JsonConverter()
            }
        };
        

        /// <summary>
        /// Writes a list of generic objects to a specified file path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path the save file to.</param>
        /// <param name="objects"> List of objects to be saved.</param>
        public static void WriteJson<T>(string path, List<T> objects)
        {

            StreamWriter writer = new StreamWriter(path);
            
            string data = JsonConvert.SerializeObject(objects, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All });
            writer.Write(data);
            
            /*
            foreach (T @object in objects)
            {
                if (@object is IPersistent)
                {
                    string data = JsonSerializer.Serialize(@object, @object.GetType(), options);
                    writer.WriteLine(@object.GetType());
                    writer.WriteLine(data);
                }
            }
            */

            writer.Close();
        }

        /// <summary>
        /// Reads a list of generic objects from a specified file path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path to file.</param>
        /// <returns>List of deserialized generic objects</returns>
        ///

        public static List<T> ReadJson<T>(string path)
        {
            StreamReader reader = new StreamReader(path);

            List<T> objects = new List<T>();

            string data = reader.ReadToEnd();
            objects = JsonConvert.DeserializeObject<List<T>>(data, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All, PreserveReferencesHandling = PreserveReferencesHandling.All });

            /*
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (line.Length > 0)
                {
                    Type dataType = Type.GetType(line);
                    T @object = (T)System.Text.Json.JsonSerializer.Deserialize(reader.ReadLine(), dataType, options);
                    objects.Add(@object);
                }
            }*/

            reader.Close();

            return objects;
        } 
    }
}
