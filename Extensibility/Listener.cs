using System.Reflection;

namespace Neo.Core.Extensibility
{
    // TODO: Add docs
    public class Listener
    {
        public MethodInfo Method { get; }
        public Plugin Plugin { get; }

        public Listener(MethodInfo method, Plugin plugin) {
            this.Method = method;
            this.Plugin = plugin;
        }
    }
}
