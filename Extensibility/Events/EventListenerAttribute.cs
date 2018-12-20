using System;

namespace Neo.Core.Extensibility.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventListenerAttribute : Attribute
    {
    }
}
