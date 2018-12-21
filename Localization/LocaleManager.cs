using System;
using System.Collections.Generic;
using System.IO;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Localization
{
    /// <summary>
    ///     Represents a key/value localization dictionary.
    ///     <para>
    ///         This class uses a thread-safe singleton lazy pattern and can only be accessed through the <see cref="Instance"/> property.
    ///     </para>
    /// </summary>
    public sealed class LocaleManager
    {
        /// <summary>
        ///     Returns the only instance of the <see cref="LocaleManager"/>.
        /// </summary>
        public static LocaleManager Instance => instance.Value;

        private static readonly Lazy<LocaleManager> instance = new Lazy<LocaleManager>(() => new LocaleManager());

        /// <summary>
        ///     Gets an existing value from the locale file.
        /// </summary>
        /// <param name="locale">The locale to load the value from.</param>
        /// <param name="key">The key connected to the value.</param>
        /// <returns>Returns the value connected to the <see cref="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the loaded locale file does not contain <see cref="locale"/> or <see cref="key"/>.</exception>
        public string this[string locale, string key] => localizedValues[locale][key];

        /// <summary>
        ///     Gets an existing value from the locale file or a default value if <see cref="key"/> doesn't exist.
        /// </summary>
        /// <param name="locale">The locale to load the value from.</param>
        /// <param name="key">The key connected to the value.</param>
        /// <param name="defaultValue">The value to return if no value exists.</param>
        /// <returns>Returns the value connected to the <see cref="key"/> or <see cref="defaultValue"/> if no value exists.</returns>
        public string this[string locale, string key, string defaultValue] {
            get {
                if (localizedValues.ContainsKey(locale)) {
                    if (localizedValues[locale].ContainsKey(key)) {
                        return localizedValues[locale][key];
                    }
                }

                return defaultValue;
            }
        }

        private Dictionary<string, SortedDictionary<string, string>> localizedValues = new Dictionary<string, SortedDictionary<string, string>>();

        private LocaleManager() { }

        /// <summary>
        ///     Gets an existing value from the locale file.
        /// </summary>
        /// <param name="locale">The locale to load the value from.</param>
        /// <param name="key">The key connected to the value.</param>
        /// <returns>Returns the value connected to the <see cref="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the loaded locale file does not contain <see cref="locale"/> or <see cref="key"/>.</exception>
        public string GetValue(string locale, string key) {
            return localizedValues[locale][key];
        }

        /// <summary>
        ///     Gets an existing value from the locale file or a default value if <see cref="key"/> doesn't exist.
        /// </summary>
        /// <param name="locale">The locale to load the value from.</param>
        /// <param name="key">The key connected to the value.</param>
        /// <param name="defaultValue">The value to return if no value exists.</param>
        /// <returns>Returns the value connected to the <see cref="key"/> or <see cref="defaultValue"/> if no value exists.</returns>
        public string GetValue(string locale, string key, string defaultValue) {
            if (localizedValues.ContainsKey(locale)) {
                if (localizedValues[locale].ContainsKey(key)) {
                    return localizedValues[locale][key];
                }
            }

            return defaultValue;
        }

        /// <summary>
        ///     Loads all existing localization dictionary from a folder.
        /// </summary>
        /// <param name="path">The path to the folder.</param>
        /// <exception cref="FormatException">Thrown if the file does not contain a valid configuration.</exception>
        public void Load(string path) {
            if (Directory.Exists(path)) {
                foreach (var file in Directory.EnumerateFiles(path)) {
                    try {
                        var localeValues = JsonConvert.DeserializeObject<SortedDictionary<string, string>>(File.ReadAllText(file));
                        var fileInfo = new FileInfo(file);

                        localizedValues.Add(fileInfo.Name.Split('.')[0], localeValues);
                    } catch {

                    }
                }
            }
        }
    }

    public static class LocaleExtensions
    {
        public static string GetValue(this User user, string key) {
            return LocaleManager.Instance.GetValue(user.Locale, key);
        }

        public static string GetValue(this User user, string key, string defaultValue) {
            return LocaleManager.Instance.GetValue(user.Locale, key, defaultValue);
        }
    }
}
