using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Projectile_Simulator
{
    /// <summary>
    /// Static class used to save objects to a file
    /// </summary>
    public static class ObjectSerializer
    {
        public static void WriteToJson<T>(string path, T @object)
        {
            string data = JsonConvert.SerializeObject(@object);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(data);
            writer.Close();
        }

        public static T ReadFromJson<T>(string path)
        {
            StreamReader reader = new StreamReader(path);
            string data = reader.ReadToEnd();
            reader.Close();
            return JsonConvert.DeserializeObject<T>(data);
        }


    }
}
