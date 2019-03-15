using System;
using System.Collections.Generic;
using System.Linq;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Newtonsoft.Json;

namespace Neo.Core.Shared
{
    /// <summary>
    ///     Represents all offline stored data of a registered <see cref="Member"/>.
    /// </summary>
    public class Account : IAttributable, IAuthorizable
    {
        /// <summary>
        ///     The attributes assigned to this <see cref="Account"/>.
        /// </summary>
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        ///     The email address of this <see cref="Account"/>.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Contains all groups this <see cref="Account"/> is a member of.
        /// </summary>
        [JsonIgnore]
        public List<Group> Groups => Pool.Server.Groups.FindAll(g => g.MemberIds.Contains(InternalId)).OrderBy(g => g.SortValue).ToList();

        /// <summary>
        ///     The <see cref="Identity"/> of this <see cref="Account"/>.
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        ///     The unique id used to identify this <see cref="Account"/>.
        /// </summary>
        public Guid InternalId { get; set; } = Guid.NewGuid();

        /// <summary>
        ///     Whether the associated <see cref="User"/> is online.
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        ///     The associated <see cref="Neo.Core.Shared.Member"/> of this <see cref="Account"/>. <c>null</c> if the user is offline.
        /// </summary>
        [JsonIgnore]
        public Member Member => (Member) Pool.Server.Users.Find(u => u.InternalId == InternalId);

        /// <summary>
        ///     The password of this <see cref="Account"/>.
        /// </summary>
        public byte[] Password { get; set; }

        /// <summary>
        ///     The permissions granted to this <see cref="Account"/>.
        /// </summary>
        public Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();
    }
}
