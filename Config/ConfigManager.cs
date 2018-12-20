using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Neo.Core.Config
{
    public sealed class ConfigManager
    {
        public static ConfigManager Instance => instance.Value;

        private static readonly Lazy<ConfigManager> instance = new Lazy<ConfigManager>(() => new ConfigManager());

        public dynamic this[string key] {
            get => configValues[key];
            set {
                configValues[key] = value;

                if (!string.IsNullOrEmpty(filePath)) {
                    Save(filePath);
                }
            }
        }

        public dynamic this[string key, dynamic defaultValue] {
            get {
                if (configValues.ContainsKey(key)) {
                    return configValues[key];
                }

                configValues[key] = defaultValue;

                if (!string.IsNullOrEmpty(filePath)) {
                    Save(filePath);
                }

                return defaultValue;
            }
        }

        private Dictionary<string, object> configValues = new Dictionary<string, object>();
        private string filePath;

        private ConfigManager() { }

        public TOut GetValue<TOut>(string key) {
            return (TOut) configValues[key];
        }

        public TOut GetValue<TOut>(string key, TOut defaultValue) {
            if (configValues.ContainsKey(key)) {
                return (TOut) configValues[key];
            }

            configValues[key] = defaultValue;

            if (!string.IsNullOrEmpty(filePath)) {
                Save(filePath);
            }

            return defaultValue;
        }

        public void Load(string path) {
            if (File.Exists(path)) {
                try {
                    configValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(path));
                    filePath = path;
                } catch {
                    throw new FormatException("This file is not a valid configuration file.");
                }
            } else {
                filePath = path;
                Save(path);
            }
        }

        public void Save(string path) {
            File.WriteAllText(path, JsonConvert.SerializeObject(configValues, Formatting.Indented));
        }

        public void SetValue(string key, dynamic value) {
            configValues[key] = value;

            if (!string.IsNullOrEmpty(filePath)) {
                Save(filePath);
            }
        }
    }
}
