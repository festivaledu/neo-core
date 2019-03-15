using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Neo.Core.Extensibility.Events;

namespace Neo.Core.Extensibility
{
    /// <summary>
    ///     Manages the registration and invoking of events and listeners.
    /// </summary>
    public static class EventService
    {
        private static readonly Dictionary<EventType, List<Listener>> listeners = new Dictionary<EventType, List<Listener>>();

        /// <summary>
        ///     Searches for <see cref="EventListenerAttribute"/>s and registers a listener to the given <see cref="Plugin"/>.
        /// </summary>
        /// <param name="type">The type of the <see cref="Plugin"/>.</param>
        /// <param name="plugin">The <see cref="Plugin"/> itself.</param>
        public static void RegisterListeners(Type type, Plugin plugin) {

            // Find all methods that have a EventListenerAttribute attached to them
            var eventMethods = type.GetMethods().ToList().FindAll(mi => mi.GetCustomAttribute<EventListenerAttribute>() != null);
            foreach (var eventMethod in eventMethods) {

                // Get the type of the listener and either add method and plugin to the existing list or create a new one if no plugin has subscribed to this event yet
                var eventType = eventMethod.GetCustomAttribute<EventListenerAttribute>().Type;
                
                if (!listeners.ContainsKey(eventType)) {
                    listeners.Add(eventType, new List<Listener> { new Listener(eventMethod, plugin) });
                } else {
                    listeners[eventType].Add(new Listener(eventMethod, plugin));
                }
            }
        }

        /// <summary>
        ///     Raises an event asynchronously and calls all registered listeners.
        /// </summary>
        /// <param name="type">The <see cref="EventType"/> to raise.</param>
        /// <param name="args">The event arguments to pass.</param>
        public static void RaiseEvent(EventType type, dynamic args) {
            if (listeners.ContainsKey(type) && listeners[type] != null) {
                foreach (var listener in listeners[type]) {
                    listener.Method.Invoke(listener.Plugin, new[] { args });
                }
            }
        }
    }
}
