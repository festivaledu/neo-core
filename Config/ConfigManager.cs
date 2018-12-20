using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WebSocketSharp;

namespace Neo.Core.Config
{
    /// <summary>
    ///     Represents a key/value config dictionary.
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

        /// <summary>
        ///     Gets an existing value from the config or sets a new one.
        ///     <para>
        ///         Setting a new value will override the existing one. If <see cref="Load"/> has been called at least once, the config will be saved automatically.
        ///     </para>
        /// </summary>
        /// <param name="key">The key connected to the value.</param>
        /// <returns>Returns the value connected to the <see cref="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the loaded config does not contain <see cref="key"/>.</exception>
        public dynamic this[string key] {
            get => configValues[key];
            set {
                configValues[key] = value;
                Save(filePath);
            }
        }

        /// <summary>
        ///     Gets an existing value from the config or a default value if <see cref="key"/> doesn't exist.
        ///     <para>
        ///         The <see cref="defaultValue"/> will be set to the <see cref="key"/> if no value existed yet. If <see cref="Load"/> has been called at least once, the config will be saved automatically.
        ///     </para>
        /// </summary>
        /// <param name="key">The key connected to the value.</param>
        /// <param name="defaultValue">The value to return and set as the default value if no value existed yet.</param>
        /// <returns>Returns the value connected to the <see cref="key"/> or <see cref="defaultValue"/> if no value existed yet.</returns>
        public dynamic this[string key, dynamic defaultValue] {
            get {
                if (configValues.ContainsKey(key)) {
                    return configValues[key];
                }

                configValues[key] = defaultValue;
                Save(filePath);

                return defaultValue;
            }
        }

        private SortedDictionary<string, object> configValues = new SortedDictionary<string, object>();
        private string filePath;

        private ConfigManager() { }

        /// <summary>
        ///     Gets an existing value from the config.
        /// </summary>
        /// <typeparam name="TOut">The type to convert the value to.</typeparam>
        /// <param name="key">The key connected to the value.</param>
        /// <returns>Returns the value connected to the <see cref="key"/> as <see cref="TOut"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the loaded config does not contain <see cref="key"/>.</exception>
        public TOut GetValue<TOut>(string key) {
            return (TOut) configValues[key];
        }

        /// <summary>
        ///     Gets an existing value from the config or a default value if <see cref="key"/> doesn't exist.
        ///     <para>
        ///         The <see cref="defaultValue"/> will be set to the <see cref="key"/> if no value existed yet. If <see cref="Load"/> has been called at least once, the config will be saved automatically.
        ///     </para>
        /// </summary>
        /// <typeparam name="TOut">The type to convert the value to.</typeparam>
        /// <param name="key">The key connected to the value.</param>
        /// <param name="defaultValue">The value to return and set as the default value if no value existed yet.</param>
        /// <returns>Returns the value connected to the <see cref="key"/> or <see cref="defaultValue"/> if no value existed yet.</returns>
        public TOut GetValue<TOut>(string key, TOut defaultValue) {
            if (configValues.ContainsKey(key)) {
                return (TOut) configValues[key];
            }

            configValues[key] = defaultValue;
            Save(filePath);

            return defaultValue;
        }

        /// <summary>
        ///     Loads an existing config from a file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <exception cref="FormatException">Thrown if the file does not contain a valid configuration.</exception>
        public void Load(string path) {
            if (File.Exists(path)) {
                try {
                    configValues = JsonConvert.DeserializeObject<SortedDictionary<string, object>>(File.ReadAllText(path));
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
        public void Save(string path) {
            if (!path.IsNullOrEmpty()) {
                File.WriteAllText(path, JsonConvert.SerializeObject(configValues, Formatting.Indented));
            }
        }

        /// <summary>
        ///     Sets a new value to the config.
        ///     <para>
        ///         Setting a new value will override the existing one. If <see cref="Load"/> has been called at least once, the config will be saved automatically.
        ///     </para>
        /// </summary>
        /// <param name="key">The key connected to the value.</param>
        /// <param name="value">The value to set.</param>
        public void SetValue(string key, dynamic value) {
            configValues[key] = value;
            Save(filePath);
        }
    }
}
