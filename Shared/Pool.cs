using Neo.Core.Database;
using Neo.Core.Networking;

namespace Neo.Core.Shared
{
    public static class Pool
    {
        public static BaseServer Server { get; set; }
        internal static DataProvider DataProvider { get; set; }
    }
}
