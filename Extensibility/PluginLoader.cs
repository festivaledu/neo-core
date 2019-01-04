using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Neo.Core.Extensibility
{
    public static class PluginLoader
    {
        public static List<Plugin> Plugins { get; } = new List<Plugin>();

        public static async Task InitializePlugin(string path) {
            if (File.Exists(path) && new FileInfo(path).Extension == ".dll") {
                Assembly.LoadFile(path);
                var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => typeof(Plugin).IsAssignableFrom(t) && t.IsClass).ToArray();

                foreach (var type in assemblyTypes) {
                    if (type.Attributes.HasFlag(TypeAttributes.Abstract)) {
                        continue;
                    }

                    var plugin = (Plugin) Activator.CreateInstance(type);
                    
                    if (Plugins.Any(p => p.Namespace == plugin.Namespace)) {
                        plugin = null;
                        continue;
                    }

                    EventService.RegisterListeners(type, plugin);

                    await plugin.OnInitialize();

                    Plugins.Add(plugin);
                }
            }
        }
    }
}
