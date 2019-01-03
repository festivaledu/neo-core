using System;
using Neo.Core.Extensibility.Events;

namespace Neo.Core.Extensibility
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
