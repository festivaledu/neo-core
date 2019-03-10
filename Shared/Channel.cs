using System;
using System.Collections.Generic;
using System.Linq;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Networking;
using Newtonsoft.Json;

namespace Neo.Core.Shared
{
    public class Channel : IAttributable
    {
        public List<Guid> ActiveMemberIds { get; set; } = new List<Guid>();

        [JsonIgnore]
        public List<User> ActiveMembers => Pool.Server.Users.FindAll(u => ActiveMemberIds.Contains(u.InternalId));

        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public List<Guid> BlacklistedGroupIds { get; set; } = new List<Guid>();

        public List<Guid> BlacklistedUserIds { get; set; } = new List<Guid>();

        public DateTime EndOfLifetime { get; set; }

        public string Id { get; set; }

        public Guid InternalId { get; set; } = Guid.NewGuid();

        public Lifespan Lifetime { get; set; } = Lifespan.Temporary;

        public int Limit { get; set; }

        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        public Dictionary<Guid, Dictionary<string, Permission>> MemberPermissions { get; set; } = new Dictionary<Guid, Dictionary<string, Permission>>();

        [JsonIgnore]
        public List<User> Members => Pool.Server.Users.FindAll(u => MemberIds.Contains(u.InternalId));

        public List<MessagePackageContent> Messages { get; set; } = new List<MessagePackageContent>();

        public string Name { get; set; }

        public Guid Owner { get; set; }

        public string Password { get; set; }

        public string StatusMessage { get; set; }

        public List<Guid> WhitelistedGroupIds { get; set; } = new List<Guid>();

        public List<Guid> WhitelistedUserIds { get; set; } = new List<Guid>();

        public List<Guid> VisibleToGroupIds { get; set; } = new List<Guid>();

        public List<Guid> VisibleToUserIds { get; set; } = new List<Guid>();

        public void AddMessage(User sender, string message) {
            var received = MessagePackageContent.GetReceivedMessage(sender.InternalId, sender.Identity, message, InternalId);
            var sent = MessagePackageContent.GetSentMessage(sender.InternalId, sender.Identity, message, InternalId);

            if (message.Contains('@')) {
                var mentions = message.Split(' ').ToList().Where(s => s.StartsWith('@') && Pool.Server.Users.Any(u => u.Identity.Id == s.Substring(1))).Select(s => s.Substring(1)).Distinct().ToList();
                new Target().AddMany(Pool.Server.Users.FindAll(u => mentions.Contains(u.Identity.Id)).ToArray()).SendPackageTo(new Package(PackageType.Mention, received));
            }

            new Target().AddMany(this).Remove(sender).SendPackageTo(new Package(PackageType.Message, received));

            if (sender.ActiveChannel == this) {
                new Target(sender).SendPackageTo(new Package(PackageType.Message, sent));
            }

            Messages.Add(received);

            if (Messages.Count > 50) {
                Messages.RemoveAt(0);
            }
        }
    }
}
