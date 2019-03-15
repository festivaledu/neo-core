using System;
using Neo.Core.Extensibility.Events;

namespace Neo.Core.Extensibility
{
    /// <summary>
    ///     Marks a method as an event listener.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EventListenerAttribute : Attribute
    {
        /// <summary>
        ///     The type of event this <see cref="EventListenerAttribute"/> is attached to.
        /// </summary>
        public EventType Type { get; }
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventListenerAttribute"/> class with the given <see cref="EventType"/>.
        /// </summary>
        /// <param name="type"></param>
        public EventListenerAttribute(EventType type) {
            this.Type = type;
        }
    }
}
