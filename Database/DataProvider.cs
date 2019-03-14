using System.Collections.Generic;
using System.Linq;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Database
{
    public abstract class DataProvider
    {
        protected BaseServer server;

        protected List<Account> accounts;
        protected List<Channel> channels;
        protected List<Group> groups;

        protected DataProvider(BaseServer server) {
            this.server = server;
        }

        public abstract void Load();
        public abstract void Save();

        protected void PrepareAccounts() {
            accounts = server.Accounts.Select(_ => JsonConvert.DeserializeObject<Account>(JsonConvert.SerializeObject(_))).ToList();

            // Remove all accounts that aren't created by a client (root account + virtual plugin accounts)
            //accounts.RemoveAll(a => a.Attributes.ContainsKey("session.neo.origin") && a.Attributes["session.neo.origin"].ToString() != "neo.client" || a.Attributes.ContainsKey("instance.neo.origin") && a.Attributes["instance.neo.origin"].ToString() != "neo.client");

            // Remove all session and instance attributes
            foreach (var account in accounts) {
                account.IsOnline = false;

                for (var i = account.Attributes.Keys.Count - 1; i >= 0; i--) {
                    var key = account.Attributes.Keys.ElementAt(i);

                    if (key.StartsWith("session.") || key.StartsWith("instance.")) {
                        account.Attributes.Remove(key);
                    }
                }
            }
        }

        protected void PrepareChannels() {
            channels = server.Channels.Select(_ => JsonConvert.DeserializeObject<Channel>(JsonConvert.SerializeObject(_))).ToList();

            foreach (var channel in channels) {
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
            groups = server.Groups.Select(_ => JsonConvert.DeserializeObject<Group>(JsonConvert.SerializeObject(_))).ToList();

            // Remove all instance attributes
            foreach (var group in groups) {
                for (var i = group.Attributes.Keys.Count - 1; i >= 0; i--) {
                    var key = group.Attributes.Keys.ElementAt(i);

                    if (key.StartsWith("instance.")) {
                        group.Attributes.Remove(key);
                    }
                }

                if (group.Attributes.ContainsKey("neo.grouptype") && group.Attributes["neo.grouptype"].ToString() == "guest") {
                    group.MemberIds.Clear();
                }
            }

            groups = groups.OrderBy(_ => _.SortValue).ToList();
        }
    }
}
