using System.Reflection;

namespace Neo.Core.Extensibility
{
    /// <summary>
    ///     Represents an event listener of a <see cref="Extensibility.Plugin"/>.
    /// </summary>
    public class Listener
    {
        /// <summary>
        ///     The handler to execute.
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        ///     The <see cref="Plugin"/> this <see cref="Listener"/> belongs to.
        /// </summary>
        public Plugin Plugin { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Listener"/> class.
        /// </summary>
        /// <param name="method">The handler to execute.</param>
        /// <param name="plugin">The <see cref="Plugin"/> this <see cref="Listener"/> belongs to.</param>
        public Listener(MethodInfo method, Plugin plugin) {
            this.Method = method;
            this.Plugin = plugin;
        }
    }
}
