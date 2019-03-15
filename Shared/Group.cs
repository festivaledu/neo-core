using System;
using System.Collections.Generic;
using System.Linq;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Newtonsoft.Json;

namespace Neo.Core.Shared
{
    /// <summary>
    ///     Represents a set of <see cref="User"/>s.
    /// </summary>
    public class Group : IAttributable, IAuthorizable
    {
        /// <summary>
        ///     The attributes assigned to this <see cref="Group"/>.
        /// </summary>
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        ///     The user-defined id of this <see cref="Group"/>.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The unique id used to identify this <see cref="Group"/>.
        /// </summary>
        public Guid InternalId { get; set; } = Guid.NewGuid();

        /// <summary>
        ///     Contains the internal ids of all members of this <see cref="Group"/>.
        /// </summary>
        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        /// <summary>
        ///     Contains all <see cref="Account"/>s that are members of this <see cref="Group"/>.
        /// </summary>
        [JsonIgnore]
        public List<Account> Members => MemberIds.Select(m => Pool.Server.Accounts.Find(a => a.InternalId == m)).ToList();

        /// <summary>
        ///     The name of this <see cref="Group"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The permissions granted to all members of this <see cref="Group"/>.
        /// </summary>
        public Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();

        /// <summary>
        ///     Determines the sort and inheritance order of this <see cref="Group"/>.
        /// </summary>
        public int SortValue { get; set; }
    }
}
