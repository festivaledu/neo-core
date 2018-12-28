using System;

namespace Neo.Core.Extensibility.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventListenerAttribute : Attribute
    {
        public EventType Type { get; }

        public EventListenerAttribute(EventType type) {
            this.Type = type;
        }
    }
}
