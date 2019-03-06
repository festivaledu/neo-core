using System;
using System.Linq;
using System.Reflection;
using Neo.Core.Communication.Packages;

namespace Neo.Core.Config
{
    public static class SettingsProvider
    {
        public static OpenSettingsResponsePackageContent OpenSettings(string settings) {
            OpenSettingsResponsePackageContent response = null;

            if (settings == "server") {
                var scope = ConfigManager.Instance.Values;
                var scopeType = typeof(ConfigValues);

                response = new OpenSettingsResponsePackageContent(scope);

                var properties = scopeType.GetProperties().ToList().FindAll(pi => pi.GetCustomAttribute<EditablePropertyAttribute>() != null);
                foreach (var property in properties) {
                    response.Titles.Add(property.Name.ToLower(), property.GetCustomAttribute<EditablePropertyAttribute>().Title);
                }
            }

            return response;
        }
    }
}
