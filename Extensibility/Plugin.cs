using System;
using System.Threading.Tasks;
using Neo.Core.Extensibility.Events;
using Neo.Core.Shared;

namespace Neo.Core.Extensibility
{
    public abstract class Plugin
    {
        public abstract string Namespace { get; }
    }
}
