using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Neo.Core.Extensibility.Events;

namespace Neo.Core.Extensibility
{
    public static class EventService
    {
        private static readonly Dictionary<EventType, List<(MethodInfo Method, Plugin Plugin)>> listeners = new Dictionary<EventType, List<(MethodInfo Method, Plugin Plugin)>>();

        public static void RegisterListeners(Type type, Plugin plugin) {
            var eventMethods = type.GetMethods().ToList().FindAll(mi => mi.GetCustomAttribute<EventListenerAttribute>() != null);
            foreach (var eventMethod in eventMethods) {
                var eventType = eventMethod.GetCustomAttribute<EventListenerAttribute>().Type;

                if (!listeners.ContainsKey(eventType)) {
                    listeners.Add(eventType, new List<(MethodInfo Method, Plugin Plugin)> { (eventMethod, plugin) });
                } else {
                    listeners[eventType].Add((eventMethod, plugin));
                }
            }
        }

        public static async Task RaiseEvent(EventType type, dynamic args) {
            foreach (var listener in listeners[type]) {
                await (Task) listener.Method.Invoke(listener.Plugin, new[] { args });
            }
        }
    }
}
