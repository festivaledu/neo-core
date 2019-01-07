using Neo.Core.Networking;

namespace Neo.Core.Database
{
    public abstract class DataProvider
    {
        protected BaseServer server;

        protected DataProvider(BaseServer server) {
            this.server = server;
        }

        public abstract void Load();
        public abstract void Save();
    }
}
