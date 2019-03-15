using Neo.Core.Networking;

namespace Neo.Core.Shared
{
    /// <summary>
    ///     Acts as an access point for the <see cref="BaseServer"/>.
    /// </summary>
    public static class Pool
    {
        /// <summary>
        ///     The operating <see cref="BaseServer"/> instance.
        /// </summary>
        public static BaseServer Server { get; set; }
    }
}
