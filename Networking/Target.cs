using System.Collections.Generic;
using System.Linq;
using Neo.Core.Shared;

namespace Neo.Core.Networking
{
    public class Target
    {
        public static Target All => new Target { targets = Pool.Server.Clients.Select(c => c.ClientId).ToList() };

        public List<string> Targets => targets.Distinct().ToList();

        private List<string> targets = new List<string>();

        public Target() { }

        public Target(string clientId) {
            targets.Add(clientId);
        }

        public Target(User user) {
            targets.Add(user.Client.ClientId);
        }

        public Target Add(string clientId) {
            targets.Add(clientId);
            return this;
        }

        public Target Add(User user) {
            targets.Add(user.Client.ClientId);
            return this;
        }

        public Target AddMany(params string[] clientIds) {
            targets.AddRange(clientIds);
            return this;
        }

        public Target AddMany(params User[] users) {
            targets.AddRange(users.Select(u => u.Client.ClientId));
            return this;
        }

        public Target AddMany(Channel channel) {
            targets.AddRange(channel.Members.Select(m => m.Client.ClientId));
            return this;
        }

        public Target Remove(string clientId) {
            targets.Remove(clientId);
            return this;
        }

        public Target Remove(User user) {
            targets.Remove(user.Client.ClientId);
            return this;
        }

        public Target RemoveMany(params string[] clientIds) {
            targets.RemoveAll(clientIds.Contains);
            return this;
        }

        public Target RemoveMany(params User[] users) {
            targets.RemoveAll(t => users.Select(u => u.Client.ClientId).Contains(t));
            return this;
        }
    }
}
