//using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace Simulator
{
    /// <summary>
    /// Static class used to serialize objects using JSON encoding and save to a file.
    /// </summary>
    public static class FileSaver
    {
        /// <summary>
        /// Writes a generic state object to a specified file path.
        /// </summary>
        /// <typeparam name="T">Type of object to write.</typeparam>
        /// <param name="path">Path the save file to.</param>
        /// <param name="state">State to write to file.</param>
        public static void WriteJson<T>(string path, T state)
        {
            StreamWriter writer = new StreamWriter(path);

            // Serialize data
            string data = SerializeJson(state);

            writer.Write(data);

            writer.Close();
        }

        /// <summary>
        /// Reads a generic state object from a specified file path.
        /// </summary>
        /// <typeparam name="T">Type to deserialize to.</typeparam>
        /// <param name="path">Path to file.</param>
        /// <returns>Deserialized generic state.</returns>
        public static T ReadJson<T>(string path)
        {
            StreamReader reader = new StreamReader(path);

            T state;

            // Attempt to open file
            string text = reader.ReadToEnd();

            // Deserialize string
            state = DeserializeJson<T>(text);

            reader.Close();

            return state;
        }

        /// <summary>
        /// Serializes generic state to JSON string.
        /// </summary>
        /// <typeparam name="T">Type of state.</typeparam>
        /// <param name="state">State to serialize.</param>
        /// <returns>Serialized JSON string.</returns>
        public static string SerializeJson<T>(T state)
        {
            string data = JsonConvert.SerializeObject(state, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });

            return data;
        }

        /// <summary>
        /// Serializes JSON string to generic state.
        /// </summary>
        /// <typeparam name="T">Type of state.</typeparam>
        /// <param name="text">JSON string to deserialize.</param>
        /// <returns>Deserialized generic state.</returns>
        public static T DeserializeJson<T>(string text)
        {
            T state;

            try
            {
                // Deserialize state to JSON string
                state = JsonConvert.DeserializeObject<T>(text, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                });
            }
            catch (JsonReaderException)
            {
                // Catch invalid or corrupted file
                MessageBox.Show("File could not be loaded. Loading default file instead.", "File invalid!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default;
            }

            return state;
        }
    }
}