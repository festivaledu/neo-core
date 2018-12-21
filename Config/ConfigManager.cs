using System;
using System.IO;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Neo.Core.Config
{
    /// <summary>
    ///     Represents a single access point for all configuration values.
    ///     <para>
    ///         This class uses a thread-safe singleton lazy pattern and can only be accessed through the <see cref="Instance"/> property.
    ///     </para>
    /// </summary>
    public sealed class ConfigManager
    {
        /// <summary>
        ///     Returns the only instance of the <see cref="ConfigManager"/>.
        /// </summary>
        public static ConfigManager Instance => instance.Value;

        private static readonly Lazy<ConfigManager> instance = new Lazy<ConfigManager>(() => new ConfigManager());
        
        public ConfigValues Values { get; set; } = new ConfigValues();
        private string filePath;

        private ConfigManager() { }

        /// <summary>
        ///     Loads an existing config from a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <exception cref="FormatException">Thrown if the file does not contain a valid configuration.</exception>
        public void Load(string path) {
            if (File.Exists(path)) {
                try {
                    Values = JsonConvert.DeserializeObject<ConfigValues>(File.ReadAllText(path));
                    filePath = path;
                } catch {
                    throw new FormatException("This file is not a valid configuration file.");
                }
            } else {
                filePath = path;
                Save(path);
            }
        }

        /// <summary>
        ///     Saves the current config to a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public void Save(string path = "") {
            if (!path.IsNullOrEmpty()) {
                File.WriteAllText(path, JsonConvert.SerializeObject(Values, Formatting.Indented));
            } else if (!filePath.IsNullOrEmpty()) {
                File.WriteAllText(filePath, JsonConvert.SerializeObject(Values, Formatting.Indented));
            }
        }
    }
}
