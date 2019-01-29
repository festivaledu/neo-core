using System.Reflection;

namespace Neo.Core.Extensibility
{
    public class EventListener
    {
        public MethodInfo Method { get; }
        public Plugin Plugin { get; }

        public EventListener(MethodInfo method, Plugin plugin) {
            this.Method = method;
            this.Plugin = plugin;
        }
    }
}
