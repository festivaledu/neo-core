using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Neo.Core.Extensibility
{
    /// <summary>
    ///     Provides methods to load <see cref="Plugin"/>s.
    /// </summary>
    public static class PluginLoader
    {
        /// <summary>
        ///     Contains all loaded <see cref="Plugin"/>s.
        /// </summary>
        public static List<Plugin> Plugins { get; } = new List<Plugin>();

        /// <summary>
        ///     Loads and initializes a <see cref="Plugin"/>.
        /// </summary>
        /// <param name="path">The path to the <see cref="Plugin"/>.</param>
        public static void InitializePlugin(string path) {
            if (File.Exists(path) && new FileInfo(path).Extension == ".dll") {
                Assembly.LoadFile(path);
                var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => typeof(Plugin).IsAssignableFrom(t) && t.IsClass).ToArray();

                foreach (var type in assemblyTypes) {
                    if (type.Attributes.HasFlag(TypeAttributes.Abstract)) {
                        continue;
                    }

                    var plugin = (Plugin) Activator.CreateInstance(type);
                    
                    if (Plugins.Any(_ => _.Namespace == plugin.Namespace)) {
                        plugin = null;
                        continue;
                    }

                    EventService.RegisterListeners(type, plugin);

                    plugin.OnInitialize(Path.Combine(Path.GetDirectoryName(path), plugin.Namespace));

                    Plugins.Add(plugin);
                }
            }
        }
    }
}
