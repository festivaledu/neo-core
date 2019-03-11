using System;
using System.Collections.Generic;
using System.Linq;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Config;
using Neo.Core.Networking;
using Newtonsoft.Json;

namespace Neo.Core.Shared
{
    /// <summary>
    ///     Represents a closed environment for sending and receiving messages.
    /// </summary>
    public class Channel : IAttributable
    {
        /// <summary>
        ///     Contains the internal ids of all users who have this <see cref="Channel"/> currently opened.
        /// </summary>
        public List<Guid> ActiveMemberIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     Contains all <see cref="User"/>s who have this <see cref="Channel"/> currently opened.
        /// </summary>
        [JsonIgnore]
        public List<User> ActiveMembers => Pool.Server.Users.FindAll(u => ActiveMemberIds.Contains(u.InternalId));

        /// <summary>
        ///     The attributes assigned to this <see cref="Channel"/>.
        /// </summary>
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        ///     Contains the internal ids of all groups that are not allowed to enter this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public List<Guid> BlacklistedGroupIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     Contains the internal ids of all users who are not allowed to enter this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public List<Guid> BlacklistedUserIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     The date this <see cref="Channel"/> should be destroyed.
        /// </summary>
        [Obsolete]
        public DateTime EndOfLifetime { get; set; }

        /// <summary>
        ///     The user-defined id of this <see cref="Channel"/>.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The unique id used to identify this <see cref="Channel"/>.
        /// </summary>
        public Guid InternalId { get; set; } = Guid.NewGuid();

        /// <summary>
        ///     Specifies the type of this <see cref="Channel"/>.
        /// </summary>
        public Lifespan Lifetime { get; set; } = Lifespan.Temporary;

        /// <summary>
        ///     The maximum allowed number of members in this <see cref="Channel"/>. <c>-1</c> means no limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///     Contains the internal ids of all users who have entered this <see cref="Channel"/>.
        /// </summary>
        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     The permissions assigned to the individual members of this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public Dictionary<Guid, Dictionary<string, Permission>> MemberPermissions { get; set; } = new Dictionary<Guid, Dictionary<string, Permission>>();

        /// <summary>
        ///     Contains all <see cref="User"/>s who have entered this <see cref="Channel"/> and are currently online.
        /// </summary>
        [JsonIgnore]
        public List<User> Members => Pool.Server.Users.FindAll(u => MemberIds.Contains(u.InternalId));

        /// <summary>
        ///     The history of messages sent in this <see cref="Channel"/>.
        /// </summary>
        public List<MessagePackageContent> Messages { get; set; } = new List<MessagePackageContent>();

        /// <summary>
        ///     The name of this <see cref="Channel"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The internal id of the owner of this <see cref="Channel"/>.
        /// </summary>
        public Guid Owner { get; set; }

        /// <summary>
        ///     The password of this <see cref="Channel"/>.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     The status message of this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     Contains the internal ids of all groups that are allowed to join this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public List<Guid> WhitelistedGroupIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     Contains the internal ids of all users who are allowed to join this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public List<Guid> WhitelistedUserIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     Contains the internal ids of all groups that are allowed to see this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public List<Guid> VisibleToGroupIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     Contains the internal ids of all users who are allowed to see this <see cref="Channel"/>.
        /// </summary>
        [Obsolete]
        public List<Guid> VisibleToUserIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     Adds a new message to this <see cref="Channel"/> and transmits it to all the current members.
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="message">The content of the message.</param>
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

            SaveMessage(received);
        }

        /// <summary>
        ///     Saves a message to the history without transmitting it.
        /// </summary>
        /// <param name="message"></param>
        public void SaveMessage(MessagePackageContent message) {
            Messages.Add(message);

            if (Messages.Count > ConfigManager.Instance.Values.MessageHistoryLimit) {
                Messages.RemoveAt(0);
            }
        }
    }
}
