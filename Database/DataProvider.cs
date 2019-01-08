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

        protected void PrepareAccounts() {
            // Remove all accounts that aren't created by a client (root account + virtual plugin accounts)
            server.Accounts.RemoveAll(a => a.Attributes.ContainsKey("session.neo.origin") && a.Attributes["session.neo.origin"].ToString() != "neo.client");

            // Remove all session and instance attributes
            foreach (var account in server.Accounts) {
                foreach (var attribute in account.Attributes.Keys) {
                    if (attribute.StartsWith("session.") || attribute.StartsWith("instance.")) {
                        account.Attributes.Remove(attribute);
                    }
                }
            }
        }

        protected void PrepareChannels() {
            // TODO: Remove expired channels

            foreach (var channel in server.Channels) {
                // Remove all active members
                channel.ActiveMemberIds.Clear();

                // Remove all instance attributes
                foreach (var attribute in channel.Attributes.Keys) {
                    if (attribute.StartsWith("instance.")) {
                        channel.Attributes.Remove(attribute);
                    }
                }
            }
        }

        protected void PrepareGroups() {
            // Remove all instance attributes
            foreach (var group in server.Groups) {
                foreach (var attribute in group.Attributes.Keys) {
                    if (attribute.StartsWith("instance.")) {
                        group.Attributes.Remove(attribute);
                    }
                }
            }
        }
    }
}
