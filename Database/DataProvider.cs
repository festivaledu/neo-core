using System.Linq;
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
            server.Accounts.RemoveAll(a => a.Attributes.ContainsKey("session.neo.origin") && a.Attributes["session.neo.origin"].ToString() != "neo.client" || a.Attributes.ContainsKey("instance.neo.origin") && a.Attributes["instance.neo.origin"].ToString() != "neo.client");

            // Remove all session and instance attributes
            foreach (var account in server.Accounts) {
                for (var i = account.Attributes.Keys.Count - 1; i >= 0; i--) {
                    var key = account.Attributes.Keys.ElementAt(i);

                    if (key.StartsWith("session.") || key.StartsWith("instance.")) {
                        account.Attributes.Remove(key);
                    }
                }
            }
        }

        protected void PrepareChannels() {
            // TODO: Remove expired channels

            // Remove main channel
            server.Channels.RemoveAll(c => c.Id == "main");

            foreach (var channel in server.Channels) {
                // Remove all active members
                channel.ActiveMemberIds.Clear();

                // Remove all instance attributes
                for (var i = channel.Attributes.Keys.Count - 1; i >= 0; i--) {
                    var key = channel.Attributes.Keys.ElementAt(i);

                    if (key.StartsWith("instance.")) {
                        channel.Attributes.Remove(key);
                    }
                }
            }
        }

        protected void PrepareGroups() {
            // Remove all instance attributes
            foreach (var group in server.Groups) {
                for (var i = group.Attributes.Keys.Count - 1; i >= 0; i--) {
                    var key = group.Attributes.Keys.ElementAt(i);

                    if (key.StartsWith("instance.")) {
                        group.Attributes.Remove(key);
                    }
                }
            }
        }
    }
}
